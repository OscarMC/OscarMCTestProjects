﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.20915.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Client.ServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Person", Namespace="http://schemas.datacontract.org/2004/07/Microsoft.Samples.ObjectReferences", IsReference=true)]
    [System.SerializableAttribute()]
    public partial class Person : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int AgeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Client.ServiceReference.Person[] FriendsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string GenderField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LocationField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Age {
            get {
                return this.AgeField;
            }
            set {
                if ((this.AgeField.Equals(value) != true)) {
                    this.AgeField = value;
                    this.RaisePropertyChanged("Age");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Client.ServiceReference.Person[] Friends {
            get {
                return this.FriendsField;
            }
            set {
                if ((object.ReferenceEquals(this.FriendsField, value) != true)) {
                    this.FriendsField = value;
                    this.RaisePropertyChanged("Friends");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Gender {
            get {
                return this.GenderField;
            }
            set {
                if ((object.ReferenceEquals(this.GenderField, value) != true)) {
                    this.GenderField = value;
                    this.RaisePropertyChanged("Gender");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Location {
            get {
                return this.LocationField;
            }
            set {
                if ((object.ReferenceEquals(this.LocationField, value) != true)) {
                    this.LocationField = value;
                    this.RaisePropertyChanged("Location");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="Microsoft.Samples.ObjectReferences", ConfigurationName="ServiceReference.ISocialNetwork")]
    public interface ISocialNetwork {
        
        [System.ServiceModel.OperationContractAttribute(Action="Microsoft.Samples.ObjectReferences/ISocialNetwork/GetPeopleInNetwork", ReplyAction="Microsoft.Samples.ObjectReferences/ISocialNetwork/GetPeopleInNetworkResponse")]
        Client.ServiceReference.Person[] GetPeopleInNetwork(Client.ServiceReference.Person p);
        
        [System.ServiceModel.OperationContractAttribute(Action="Microsoft.Samples.ObjectReferences/ISocialNetwork/GetMutualFriends", ReplyAction="Microsoft.Samples.ObjectReferences/ISocialNetwork/GetMutualFriendsResponse")]
        Client.ServiceReference.Person[] GetMutualFriends(Client.ServiceReference.Person p);
        
        [System.ServiceModel.OperationContractAttribute(Action="Microsoft.Samples.ObjectReferences/ISocialNetwork/GetCommonFriends", ReplyAction="Microsoft.Samples.ObjectReferences/ISocialNetwork/GetCommonFriendsResponse")]
        Client.ServiceReference.Person[] GetCommonFriends(Client.ServiceReference.Person[] p);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISocialNetworkChannel : Client.ServiceReference.ISocialNetwork, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SocialNetworkClient : System.ServiceModel.ClientBase<Client.ServiceReference.ISocialNetwork>, Client.ServiceReference.ISocialNetwork {
        
        public SocialNetworkClient() {
        }
        
        public SocialNetworkClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SocialNetworkClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SocialNetworkClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SocialNetworkClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Client.ServiceReference.Person[] GetPeopleInNetwork(Client.ServiceReference.Person p) {
            return base.Channel.GetPeopleInNetwork(p);
        }
        
        public Client.ServiceReference.Person[] GetMutualFriends(Client.ServiceReference.Person p) {
            return base.Channel.GetMutualFriends(p);
        }
        
        public Client.ServiceReference.Person[] GetCommonFriends(Client.ServiceReference.Person[] p) {
            return base.Channel.GetCommonFriends(p);
        }
    }
}
