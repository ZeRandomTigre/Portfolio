﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ACW_08346_464814_Client.ACW_Service_Reference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ACW_Service_Reference.ACW_Interface_Service")]
    public interface ACW_Interface_Service {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ACW_Interface_Service/HELLOMessage", ReplyAction="http://tempuri.org/ACW_Interface_Service/HELLOMessageResponse")]
        string HELLOMessage(string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ACW_Interface_Service/HELLOMessage", ReplyAction="http://tempuri.org/ACW_Interface_Service/HELLOMessageResponse")]
        System.Threading.Tasks.Task<string> HELLOMessageAsync(string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ACW_Interface_Service/PUBKEYMessage", ReplyAction="http://tempuri.org/ACW_Interface_Service/PUBKEYMessageResponse")]
        string PUBKEYMessage(string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ACW_Interface_Service/PUBKEYMessage", ReplyAction="http://tempuri.org/ACW_Interface_Service/PUBKEYMessageResponse")]
        System.Threading.Tasks.Task<string> PUBKEYMessageAsync(string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ACW_Interface_Service/SORTMessage", ReplyAction="http://tempuri.org/ACW_Interface_Service/SORTMessageResponse")]
        string SORTMessage(string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ACW_Interface_Service/SORTMessage", ReplyAction="http://tempuri.org/ACW_Interface_Service/SORTMessageResponse")]
        System.Threading.Tasks.Task<string> SORTMessageAsync(string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ACW_Interface_Service/ENCMessage", ReplyAction="http://tempuri.org/ACW_Interface_Service/ENCMessageResponse")]
        void ENCMessage(byte[] message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ACW_Interface_Service/ENCMessage", ReplyAction="http://tempuri.org/ACW_Interface_Service/ENCMessageResponse")]
        System.Threading.Tasks.Task ENCMessageAsync(byte[] message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ACW_Interface_Service/SHA1Message", ReplyAction="http://tempuri.org/ACW_Interface_Service/SHA1MessageResponse")]
        string SHA1Message(string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ACW_Interface_Service/SHA1Message", ReplyAction="http://tempuri.org/ACW_Interface_Service/SHA1MessageResponse")]
        System.Threading.Tasks.Task<string> SHA1MessageAsync(string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ACW_Interface_Service/SHA256Message", ReplyAction="http://tempuri.org/ACW_Interface_Service/SHA256MessageResponse")]
        string SHA256Message(string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ACW_Interface_Service/SHA256Message", ReplyAction="http://tempuri.org/ACW_Interface_Service/SHA256MessageResponse")]
        System.Threading.Tasks.Task<string> SHA256MessageAsync(string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ACW_Interface_Service/SIGNMessage", ReplyAction="http://tempuri.org/ACW_Interface_Service/SIGNMessageResponse")]
        string SIGNMessage(string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ACW_Interface_Service/SIGNMessage", ReplyAction="http://tempuri.org/ACW_Interface_Service/SIGNMessageResponse")]
        System.Threading.Tasks.Task<string> SIGNMessageAsync(string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ACW_Interface_ServiceChannel : ACW_08346_464814_Client.ACW_Service_Reference.ACW_Interface_Service, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ACW_Interface_ServiceClient : System.ServiceModel.ClientBase<ACW_08346_464814_Client.ACW_Service_Reference.ACW_Interface_Service>, ACW_08346_464814_Client.ACW_Service_Reference.ACW_Interface_Service {
        
        public ACW_Interface_ServiceClient() {
        }
        
        public ACW_Interface_ServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ACW_Interface_ServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ACW_Interface_ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ACW_Interface_ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string HELLOMessage(string message) {
            return base.Channel.HELLOMessage(message);
        }
        
        public System.Threading.Tasks.Task<string> HELLOMessageAsync(string message) {
            return base.Channel.HELLOMessageAsync(message);
        }
        
        public string PUBKEYMessage(string message) {
            return base.Channel.PUBKEYMessage(message);
        }
        
        public System.Threading.Tasks.Task<string> PUBKEYMessageAsync(string message) {
            return base.Channel.PUBKEYMessageAsync(message);
        }
        
        public string SORTMessage(string message) {
            return base.Channel.SORTMessage(message);
        }
        
        public System.Threading.Tasks.Task<string> SORTMessageAsync(string message) {
            return base.Channel.SORTMessageAsync(message);
        }
        
        public void ENCMessage(byte[] message) {
            base.Channel.ENCMessage(message);
        }
        
        public System.Threading.Tasks.Task ENCMessageAsync(byte[] message) {
            return base.Channel.ENCMessageAsync(message);
        }
        
        public string SHA1Message(string message) {
            return base.Channel.SHA1Message(message);
        }
        
        public System.Threading.Tasks.Task<string> SHA1MessageAsync(string message) {
            return base.Channel.SHA1MessageAsync(message);
        }
        
        public string SHA256Message(string message) {
            return base.Channel.SHA256Message(message);
        }
        
        public System.Threading.Tasks.Task<string> SHA256MessageAsync(string message) {
            return base.Channel.SHA256MessageAsync(message);
        }
        
        public string SIGNMessage(string message) {
            return base.Channel.SIGNMessage(message);
        }
        
        public System.Threading.Tasks.Task<string> SIGNMessageAsync(string message) {
            return base.Channel.SIGNMessageAsync(message);
        }
    }
}
