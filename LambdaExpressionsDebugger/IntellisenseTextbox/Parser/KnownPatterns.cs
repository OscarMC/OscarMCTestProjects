using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntellisenseTextbox
{
    public static class KnownPatterns
    {
        public static Pattern<Token> IsTypeDef
        {
            get
            {
                return new Pattern<Token>()
                {
                    Check =
                        (token) =>
                        {
                            return
                                token.ElementType == TokenElementType.TextElement &&
                                token.PreviousIncludingCommentsAndSpaces != null &&
                                token.PreviousIncludingCommentsAndSpaces.IsSpace &&
                                token.Previous != null &&
                                (token.Previous.Content == TokenContent.Type || token.Previous.Content == TokenContent.KeywordType);
                        }
                };
            }
        }

        public static Pattern<ParseResult> IsAutoCompleteListSituation
        {
            get
            {
                return new Pattern<ParseResult>()
                {
                    Check =
                        (parseResult) =>
                        {
                            return
                                (
                                    parseResult.CursorIsAtEndOfToken &&
                                    parseResult.CurrentToken.Length > 0 &&
                                    !parseResult.CurrentToken.IsSpace &&
                                    parseResult.CurrentToken.ElementType == TokenElementType.TextElement &&
                                    !KnownPatterns.IsTypeDef.Check(parseResult.CurrentToken)
                                ) || (
                                    parseResult.CurrentToken.IsSpace &&
                                    parseResult.CurrentToken.Previous != null &&
                                    parseResult.CurrentToken.Previous.Content == TokenContent.NewKeyword
                                ) || (
                                    KnownPatterns.IsInSubItemsListPosition.Check(parseResult)
                                );
                        }
                };
            }
        }

        public static Pattern<ParseResult> IsInSubItemsListPosition
        {
            get
            {
                return new Pattern<ParseResult>()
                {
                    Check =
                        (parseResult) =>
                        {
                            return
                                parseResult.CurrentToken.Content == TokenContent.Point &&
                                parseResult.CurrentToken.Previous != null &&
                                (parseResult.CurrentToken.Previous.Content == TokenContent.Type ||
                                parseResult.CurrentToken.Previous.Content == TokenContent.KeywordType ||
                                parseResult.CurrentToken.Previous.Content == TokenContent.TypeInstance);
                        }
                };
            }
        }

    }
}
