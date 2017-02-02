using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntellisenseTextbox
{
    public class TextFridge
    {
        public TextFridge(string originalText)
        {
            OriginalText = originalText;
            TextFridgeInternalReplaceText = "_$_TextFridge_$_";
            TextRegionConfigs = new List<FridgeTextRegionConfig>();
            EscapeSequences = new List<string>();
        }

        public const string SpecialSeperatorString = "\0";
        public List<string> EscapeSequences { get; set; }        

        private Dictionary<string, FridgeStorageObject> m_stringFridge = new Dictionary<string, FridgeStorageObject>();

        private int m_frozenItemsCount = 0;
        public int FrozenItemsCount
        {
            get { return m_frozenItemsCount; }
            private set { m_frozenItemsCount = value; }
        }

        public string OriginalText { get; set; }

        public List<FridgeTextRegionConfig> TextRegionConfigs { get; set; }

        

        public string DefrostIfFrozen(string textToDefrost)
        {
            if (m_stringFridge.ContainsKey(textToDefrost))
                return m_stringFridge[textToDefrost].ToString();
            else
                return textToDefrost;
        }

        public void RegisterTextRegion(string startSeperator, string endSeperator)
        {
            RegisterTextRegion(new[] { startSeperator }, new[] { endSeperator }, true, true);
        }

        public void RegisterTextRegion(IEnumerable<string> startSeperators, string endSeperator)
        {
            RegisterTextRegion(startSeperators, new[] { endSeperator }, true, true);
        }

        public void RegisterTextRegion(string startSeperator, IEnumerable<string> endSeperators)
        {
            RegisterTextRegion(new[] { startSeperator }, endSeperators, true, true);
        }

        public void RegisterTextRegion(string seperator)
        {
            var seperators = new[] { seperator };
            RegisterTextRegion(seperators, seperators, true, true);
        }

        public void RegisterTextRegion(IEnumerable<string> seperators)
        {
            RegisterTextRegion(seperators, seperators, true, true);
        }

        public void RegisterTextRegion(
            IEnumerable<string> startSeperators,
            IEnumerable<string> endSeperators,
            bool removeStartSeperatorFromSourceText,
            bool removeEndSeperatorFromSourceText)
        {
            TextRegionConfigs.Add(
                new FridgeTextRegionConfig()
                {
                    StartSeperators = startSeperators.ToList(),
                    EndSeperators = endSeperators.ToList(),
                    LeaveEndSeperatorInSourceText = !removeEndSeperatorFromSourceText,
                    LeaveStartSeperatorInSourceText = !removeStartSeperatorFromSourceText
                });
        }

        public string GetFrozenText()
        {
            string sourceText = OriginalText;
            if (EscapeSequences.Count > 0)
            {
                for(int i = 0; i < EscapeSequences.Count; i++)
                    sourceText = sourceText.Replace(EscapeSequences[i], ((char)(i+1)).ToString());
            }

            IEnumerable<string> startSeperators =
                from c in TextRegionConfigs
                from s in c.StartSeperators
                select s;
            IEnumerable<string> endSeperators = new List<string>();
            IEnumerable<string> seperators =
                from c in TextRegionConfigs
                from s in c.StartSeperators.Union(c.EndSeperators)
                select s;
            List<string> splitResult = sourceText.Split(seperators);

            m_stringFridge = new Dictionary<string, FridgeStorageObject>();

            StringBuilder cleanedString = new StringBuilder();
            bool insideSpecialTextAerea = false;
            string lastOccurredStartSeperator = null;
            string lastOccurredEndSeperator = string.Empty;
            string contentsToPreserve = string.Empty;
            FridgeTextRegionConfig currentTextRegionConfig = null;
            foreach (string currentTextToken in splitResult)
            {
                if (startSeperators.Contains(currentTextToken) && !insideSpecialTextAerea)
                {
                    insideSpecialTextAerea = true;
                    lastOccurredStartSeperator = currentTextToken;
                    contentsToPreserve = string.Empty;
                    currentTextRegionConfig = (from c in TextRegionConfigs
                                               where c.StartSeperators.Contains(currentTextToken)
                                               select c).FirstOrDefault();
                    endSeperators = currentTextRegionConfig.EndSeperators;
                }
                else if (endSeperators.Contains(currentTextToken) && insideSpecialTextAerea)
                {
                    insideSpecialTextAerea = false;
                    lastOccurredEndSeperator = currentTextToken;
                    string replaceName = 
                        StoreInFridge(
                            contentsToPreserve, 
                            lastOccurredStartSeperator, 
                            lastOccurredEndSeperator, 
                            currentTextRegionConfig.LeaveStartSeperatorInSourceText, 
                            currentTextRegionConfig.LeaveEndSeperatorInSourceText);
                    if (currentTextRegionConfig.LeaveStartSeperatorInSourceText)
                        cleanedString.Append(lastOccurredStartSeperator);
                    cleanedString.Append(replaceName);
                    if (currentTextRegionConfig.LeaveEndSeperatorInSourceText)
                        cleanedString.Append(lastOccurredEndSeperator);
                }
                else
                {
                    if (insideSpecialTextAerea)
                        contentsToPreserve += currentTextToken;
                    else
                        cleanedString.Append(currentTextToken);
                }
            }
            if (insideSpecialTextAerea)
            {
                string replaceName =
                    StoreInFridge(
                        contentsToPreserve,
                        lastOccurredStartSeperator,
                        lastOccurredEndSeperator,
                        currentTextRegionConfig.LeaveStartSeperatorInSourceText,
                        currentTextRegionConfig.LeaveEndSeperatorInSourceText);
                if (currentTextRegionConfig.LeaveStartSeperatorInSourceText)
                    cleanedString.Append(lastOccurredStartSeperator);
                cleanedString.Append(replaceName);
                if (currentTextRegionConfig.LeaveEndSeperatorInSourceText)
                    cleanedString.Append(lastOccurredEndSeperator);
            }

            string result = cleanedString.ToString();

            if (EscapeSequences.Count > 0)
            {
                for (int i = 0; i < EscapeSequences.Count; i++)
                    result = result.Replace(((char)(i+1)).ToString(), EscapeSequences[i]);
            }

            return result;
        }

        private string StoreInFridge(
            string currentTextToken,
            string lastOccurredStartSeperator,
            string lastOccurredEndSeperator,
            bool leaveStartSeperator,
            bool leaveEndSeperator)
        {
            FrozenItemsCount++;
            string replaceName = TextFridgeInternalReplaceText + FrozenItemsCount.ToString();            

            string textForFridging = currentTextToken;
            if (EscapeSequences.Count > 0)
            {
                for (int i = 0; i < EscapeSequences.Count; i++)
                    textForFridging = textForFridging.Replace(((char)(i + 1)).ToString(), EscapeSequences[i]);
            }

            FridgeStorageObject storageObject = new FridgeStorageObject()
            {
                Text = textForFridging,
                Prefix = lastOccurredStartSeperator,
                RestorePrefix = !leaveStartSeperator,
                Postfix = lastOccurredEndSeperator,
                RestorePostfix = !leaveEndSeperator
            };

            m_stringFridge.Add(replaceName, storageObject);
            return SpecialSeperatorString + replaceName + SpecialSeperatorString;
        }

        public string TextFridgeInternalReplaceText { get; set; }

        private class FridgeStorageObject
        {
            public string Prefix { get; set; }
            public bool RestorePrefix { get; set; }
            public string Text { get; set; }
            public string Postfix { get; set; }
            public bool RestorePostfix { get; set; }

            public override bool Equals(object obj)
            {
                return Text.Equals(obj);
            }

            public override int GetHashCode()
            {
                return Text.GetHashCode();
            }

            public override string ToString()
            {
                return ApplyPreAndPostFix(Text);
            }

            public string ApplyPreAndPostFix(string text)
            {
                return (RestorePrefix ? Prefix : "") + text + (RestorePostfix ? Postfix : "");
            }
        }

        public class FridgeTextRegionConfig
        {
            private List<string> m_startSeperators = new List<string>();

            public List<string> StartSeperators
            {
                get { return m_startSeperators; }
                set { m_startSeperators = value; }
            }

            public bool LeaveStartSeperatorInSourceText { get; set; }

            private List<string> m_endSeperators = new List<string>();

            public List<string> EndSeperators
            {
                get { return m_endSeperators; }
                set { m_endSeperators = value; }
            }

            public bool LeaveEndSeperatorInSourceText { get; set; }

        }

    }
}
