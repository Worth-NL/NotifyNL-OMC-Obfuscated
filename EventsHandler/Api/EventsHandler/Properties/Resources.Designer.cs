﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EventsHandler.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EventsHandler.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OMC.
        /// </summary>
        internal static string Application_Name {
            get {
                return ResourceManager.GetString("Application_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specific type of settings cannot be initialized..
        /// </summary>
        internal static string Configuration_ERROR_CannotInitializeSettings {
            get {
                return ResourceManager.GetString("Configuration_ERROR_CannotInitializeSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to In the settings representing domain unnecessary endpoint (.../get/something) was found:.
        /// </summary>
        internal static string Configuration_ERROR_ContainsEndpoint {
            get {
                return ResourceManager.GetString("Configuration_ERROR_ContainsEndpoint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to In the settings representing domain unnecessary protocol (http or https) was found:.
        /// </summary>
        internal static string Configuration_ERROR_ContainsHttp {
            get {
                return ResourceManager.GetString("Configuration_ERROR_ContainsHttp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The retrieving of Environment Variable failed. The operating system (OS) is not yet supported..
        /// </summary>
        internal static string Configuration_ERROR_EnvironmentNotSupported {
            get {
                return ResourceManager.GetString("Configuration_ERROR_EnvironmentNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The looked up Environment Variable could not be found or the key is missing / not existing..
        /// </summary>
        internal static string Configuration_ERROR_EnvironmentVariableGetNull {
            get {
                return ResourceManager.GetString("Configuration_ERROR_EnvironmentVariableGetNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to In the settings the Template ID is invalid (should be UUID: 00000000-0000-0000-0000-000000000000):.
        /// </summary>
        internal static string Configuration_ERROR_InvalidTemplateId {
            get {
                return ResourceManager.GetString("Configuration_ERROR_InvalidTemplateId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to It was not possible to determine the given data provider. It might be not implemented yet..
        /// </summary>
        internal static string Configuration_ERROR_Loader_NotImplemented {
            get {
                return ResourceManager.GetString("Configuration_ERROR_Loader_NotImplemented", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to It was not possible to retrieve any data provider. The loading service might not be set..
        /// </summary>
        internal static string Configuration_ERROR_Loader_NotSet {
            get {
                return ResourceManager.GetString("Configuration_ERROR_Loader_NotSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The version of OpenKlant service to be used by OMC is unknown or not supported (&apos;Features:OpenServicesVersion&apos;)..
        /// </summary>
        internal static string Configuration_ERROR_OpenKlantVersionUnknown {
            get {
                return ResourceManager.GetString("Configuration_ERROR_OpenKlantVersionUnknown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The version of OpenZaak service to be used by OMC is unknown or not supported (&apos;Features:OpenServicesVersion&apos;)..
        /// </summary>
        internal static string Configuration_ERROR_OpenZaakVersionUnknown {
            get {
                return ResourceManager.GetString("Configuration_ERROR_OpenZaakVersionUnknown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The settings does not contain a given value, or the value is empty:.
        /// </summary>
        internal static string Configuration_ERROR_ValueNotFoundOrEmpty {
            get {
                return ResourceManager.GetString("Configuration_ERROR_ValueNotFoundOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The given value cannot be deserialized into dedicated target object.
        /// </summary>
        internal static string Deserialization_ERROR_CannotDeserialize_Message {
            get {
                return ResourceManager.GetString("Deserialization_ERROR_CannotDeserialize_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Target.
        /// </summary>
        internal static string Deserialization_ERROR_CannotDeserialize_Target {
            get {
                return ResourceManager.GetString("Deserialization_ERROR_CannotDeserialize_Target", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value.
        /// </summary>
        internal static string Deserialization_ERROR_CannotDeserialize_Value {
            get {
                return ResourceManager.GetString("Deserialization_ERROR_CannotDeserialize_Value", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The JSON payload is invalid..
        /// </summary>
        internal static string Deserialization_ERROR_InvalidJson_Message {
            get {
                return ResourceManager.GetString("Deserialization_ERROR_InvalidJson_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SENDER: The input cannot be recognized as JSON format..
        /// </summary>
        internal static string Deserialization_ERROR_InvalidJson_Reason1 {
            get {
                return ResourceManager.GetString("Deserialization_ERROR_InvalidJson_Reason1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Required properties are missing in the given JSON payload..
        /// </summary>
        internal static string Deserialization_ERROR_NotDeserialized_Notification_Properties_Message {
            get {
                return ResourceManager.GetString("Deserialization_ERROR_NotDeserialized_Notification_Properties_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SENDER: The standard JSON schema was recently changed and some mandatory properties were removed from it..
        /// </summary>
        internal static string Deserialization_ERROR_NotDeserialized_Notification_Properties_Reason1 {
            get {
                return ResourceManager.GetString("Deserialization_ERROR_NotDeserialized_Notification_Properties_Reason1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to RECEIVER: In the POCO model new [Required] properties were added, causing a mismatch with the standard JSON schema..
        /// </summary>
        internal static string Deserialization_ERROR_NotDeserialized_Notification_Properties_Reason2 {
            get {
                return ResourceManager.GetString("Deserialization_ERROR_NotDeserialized_Notification_Properties_Reason2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Received value could not be recognized (might be unexpected)..
        /// </summary>
        internal static string Deserialization_ERROR_NotDeserialized_Notification_Value_Message {
            get {
                return ResourceManager.GetString("Deserialization_ERROR_NotDeserialized_Notification_Value_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SENDER: In the JSON schema some data (value) of property (key) has a different type, format, or is out of range than supported in the POCO model..
        /// </summary>
        internal static string Deserialization_ERROR_NotDeserialized_Notification_Value_Reason1 {
            get {
                return ResourceManager.GetString("Deserialization_ERROR_NotDeserialized_Notification_Value_Reason1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to RECEIVER: In the POCO model the type, format or range of expected data was recently changed, causing a mismatch with the JSON schema..
        /// </summary>
        internal static string Deserialization_ERROR_NotDeserialized_Notification_Value_Reason2 {
            get {
                return ResourceManager.GetString("Deserialization_ERROR_NotDeserialized_Notification_Value_Reason2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Some values of properties in the nested &apos;attributes&apos; (&apos;kenmerken&apos;) in the POCO model are missing..
        /// </summary>
        internal static string Deserialization_INFO_NotDeserialized_Attributes_Properties_Message {
            get {
                return ResourceManager.GetString("Deserialization_INFO_NotDeserialized_Attributes_Properties_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SENDER: In the JSON schema some properties from the nested &apos;attributes&apos; (&apos;kenmerken&apos;) are missing, although they were previously defined in the POCO model..
        /// </summary>
        internal static string Deserialization_INFO_NotDeserialized_Attributes_Properties_Reason1 {
            get {
                return ResourceManager.GetString("Deserialization_INFO_NotDeserialized_Attributes_Properties_Reason1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SENDER: In the JSON schema some keys of properties in the nested &apos;attributes&apos; (&apos;kenmerken&apos;) were renamed, causing mismatch with the POCO model..
        /// </summary>
        internal static string Deserialization_INFO_NotDeserialized_Attributes_Properties_Reason2 {
            get {
                return ResourceManager.GetString("Deserialization_INFO_NotDeserialized_Attributes_Properties_Reason2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to RECEIVER: In the POCO model the custom names of some attributes are different from the keys of properties in the nested &apos;attributes&apos; (&apos;kenmerken&apos;) in the received JSON payload..
        /// </summary>
        internal static string Deserialization_INFO_NotDeserialized_Attributes_Properties_Reason3 {
            get {
                return ResourceManager.GetString("Deserialization_INFO_NotDeserialized_Attributes_Properties_Reason3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The JSON payload contains more properties in the nested &apos;attributes&apos; (&apos;kenmerken&apos;) than expected by the POCO model..
        /// </summary>
        internal static string Deserialization_INFO_UnexpectedData_Attributes_Message {
            get {
                return ResourceManager.GetString("Deserialization_INFO_UnexpectedData_Attributes_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SENDER: The JSON schema was recently changed and some properties in the nested &apos;attributes&apos; (&apos;kenmerken&apos;) were added to it..
        /// </summary>
        internal static string Deserialization_INFO_UnexpectedData_Attributes_Reason1 {
            get {
                return ResourceManager.GetString("Deserialization_INFO_UnexpectedData_Attributes_Reason1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to RECEIVER: In the POCO model some existing properties were removed from the nested &apos;attributes&apos;, causing a mismatch with the JSON schema..
        /// </summary>
        internal static string Deserialization_INFO_UnexpectedData_Attributes_Reason2 {
            get {
                return ResourceManager.GetString("Deserialization_INFO_UnexpectedData_Attributes_Reason2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The JSON payload contains more root &apos;notification&apos; properties than expected by the POCO model..
        /// </summary>
        internal static string Deserialization_INFO_UnexpectedData_Notification_Message {
            get {
                return ResourceManager.GetString("Deserialization_INFO_UnexpectedData_Notification_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SENDER: The JSON schema was recently changed and some root &apos;notification&apos; properties were added to it..
        /// </summary>
        internal static string Deserialization_INFO_UnexpectedData_Notification_Reason1 {
            get {
                return ResourceManager.GetString("Deserialization_INFO_UnexpectedData_Notification_Reason1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to RECEIVER: In the POCO model some existing properties were removed from the root &apos;notification&apos;, causing a mismatch with the JSON schema..
        /// </summary>
        internal static string Deserialization_INFO_UnexpectedData_Notification_Reason2 {
            get {
                return ResourceManager.GetString("Deserialization_INFO_UnexpectedData_Notification_Reason2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Version of the API was requested.
        /// </summary>
        internal static string Events_ApiVersionRequested {
            get {
                return ResourceManager.GetString("Events_ApiVersionRequested", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unexpected error occurred during processing the notification with ID.
        /// </summary>
        internal static string Feedback_NotifyNL_ERROR_UnexpectedFailure {
            get {
                return ResourceManager.GetString("Feedback_NotifyNL_ERROR_UnexpectedFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The status of notification with ID.
        /// </summary>
        internal static string Feedback_NotifyNL_SUCCESS_NotificationStatus {
            get {
                return ResourceManager.GetString("Feedback_NotifyNL_SUCCESS_NotificationStatus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HTTP Request: Results representing case roles (retrieved from OpenZaak Web service) are empty..
        /// </summary>
        internal static string HttpRequest_ERROR_EmptyCaseRoles {
            get {
                return ResourceManager.GetString("HttpRequest_ERROR_EmptyCaseRoles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HTTP Request: Results representing citizen results (retrieved from OpenKlant Web service) are empty..
        /// </summary>
        internal static string HttpRequest_ERROR_EmptyCitizenResults {
            get {
                return ResourceManager.GetString("HttpRequest_ERROR_EmptyCitizenResults", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The HTTPS protocol is expected..
        /// </summary>
        internal static string HttpRequest_ERROR_HttpsProtocolExpected {
            get {
                return ResourceManager.GetString("HttpRequest_ERROR_HttpsProtocolExpected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The HTTP Request sent to external Web service failed..
        /// </summary>
        internal static string HttpRequest_ERROR_Message {
            get {
                return ResourceManager.GetString("HttpRequest_ERROR_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HTTP Request: Any case role (retrieved from OpenZaak Web service) does not have initiator role matching to the one specified in the project configuration (&apos;OmschrijvingGeneriek&apos;). There is no initiator at all..
        /// </summary>
        internal static string HttpRequest_ERROR_MissingInitiatorRole {
            get {
                return ResourceManager.GetString("HttpRequest_ERROR_MissingInitiatorRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HTTP Request: Multiple roles (retrieved from OpenZaak Web service) have the same initiator role matching to the one specified in the project configuration (&apos;OmschrijvingGeneriek&apos;). It cannot be determined which of them is the initiator..
        /// </summary>
        internal static string HttpRequest_ERROR_MultipleInitiatorRoles {
            get {
                return ResourceManager.GetString("HttpRequest_ERROR_MultipleInitiatorRoles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HTTP Request: Case could not be retrieved from OpenZaak Web service..
        /// </summary>
        internal static string HttpRequest_ERROR_NoCase {
            get {
                return ResourceManager.GetString("HttpRequest_ERROR_NoCase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HTTP Request: Case details could not be retrieved from OpenZaak Web service..
        /// </summary>
        internal static string HttpRequest_ERROR_NoCaseDetails {
            get {
                return ResourceManager.GetString("HttpRequest_ERROR_NoCaseDetails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HTTP Request: Case role could not be retrieved from OpenZaak Web service..
        /// </summary>
        internal static string HttpRequest_ERROR_NoCaseRole {
            get {
                return ResourceManager.GetString("HttpRequest_ERROR_NoCaseRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HTTP Request: Case statuses could not be retrieved from OpenZaak Web service..
        /// </summary>
        internal static string HttpRequest_ERROR_NoCaseStatuses {
            get {
                return ResourceManager.GetString("HttpRequest_ERROR_NoCaseStatuses", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HTTP Request: Case status type could not be retrieved from OpenZaak Web service..
        /// </summary>
        internal static string HttpRequest_ERROR_NoCaseStatusType {
            get {
                return ResourceManager.GetString("HttpRequest_ERROR_NoCaseStatusType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HTTP Request: Citizen details could not be retrieved from OpenKlant Web service..
        /// </summary>
        internal static string HttpRequest_ERROR_NoCitizenDetails {
            get {
                return ResourceManager.GetString("HttpRequest_ERROR_NoCitizenDetails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HTTP Request: The contact moment could not be retrieved from OpenKlant Web service..
        /// </summary>
        internal static string HttpRequest_ERROR_NoFeedbackKlant {
            get {
                return ResourceManager.GetString("HttpRequest_ERROR_NoFeedbackKlant", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HTTP Request: The case (obtained from OpenZaak Web service) does not contain any statuses..
        /// </summary>
        internal static string HttpRequest_ERROR_NoLastStatus {
            get {
                return ResourceManager.GetString("HttpRequest_ERROR_NoLastStatus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HTTP Request: The main object could not be retrieved from OpenZaak Web service..
        /// </summary>
        internal static string HttpRequest_ERROR_NoMainObject {
            get {
                return ResourceManager.GetString("HttpRequest_ERROR_NoMainObject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HTTP Request: The notification (from OpenNotificaties Web service) does not contain source organization (&apos;bronorganisatie&apos;)..
        /// </summary>
        internal static string HttpRequest_ERROR_NoSourceOrganization {
            get {
                return ResourceManager.GetString("HttpRequest_ERROR_NoSourceOrganization", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to RECEIVER: The requested resource is unavailable or not existing..
        /// </summary>
        internal static string HttpRequest_ERROR_Reason1 {
            get {
                return ResourceManager.GetString("HttpRequest_ERROR_Reason1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SENDER: The network configuration is improper..
        /// </summary>
        internal static string HttpRequest_ERROR_Reason2 {
            get {
                return ResourceManager.GetString("HttpRequest_ERROR_Reason2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unknown validation issue occurred..
        /// </summary>
        internal static string Operation_ERROR_Unknown_ValidationIssue_Message {
            get {
                return ResourceManager.GetString("Operation_ERROR_Unknown_ValidationIssue_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Notification could not be recognized (deserialized)..
        /// </summary>
        internal static string Operation_RESULT_Deserialization_Failure {
            get {
                return ResourceManager.GetString("Operation_RESULT_Deserialization_Failure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Notification was partially recognized (deserialized)..
        /// </summary>
        internal static string Operation_RESULT_Deserialization_Partial {
            get {
                return ResourceManager.GetString("Operation_RESULT_Deserialization_Partial", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Notification was successfully recognized (deserialized)..
        /// </summary>
        internal static string Operation_RESULT_Deserialization_Success {
            get {
                return ResourceManager.GetString("Operation_RESULT_Deserialization_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bad request..
        /// </summary>
        internal static string Operation_RESULT_HttpRequest_Failure {
            get {
                return ResourceManager.GetString("Operation_RESULT_HttpRequest_Failure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unknown issue occurred. Internal server error..
        /// </summary>
        internal static string Operation_RESULT_Internal {
            get {
                return ResourceManager.GetString("Operation_RESULT_Internal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This operation is not implemented. Internal server error..
        /// </summary>
        internal static string Operation_RESULT_NotImplemented {
            get {
                return ResourceManager.GetString("Operation_RESULT_NotImplemented", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to It wasn&apos;t possible to extract human-friendly error message.
        /// </summary>
        internal static string Processing_ERROR_ExecutingContext_UnknownErrorDetails {
            get {
                return ResourceManager.GetString("Processing_ERROR_ExecutingContext_UnknownErrorDetails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This controller is not yet registered as handled StandardizeApiResponse.
        /// </summary>
        internal static string Processing_ERROR_ExecutingContext_UnregisteredApiController {
            get {
                return ResourceManager.GetString("Processing_ERROR_ExecutingContext_UnregisteredApiController", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The notification method (&apos;aanmaakkanaal&apos;) is unknown. The citizen data are incomplete. There should be declared at least one distribution channel: SMS, e-mail, Both, or None..
        /// </summary>
        internal static string Processing_ERROR_Notification_DeliveryMethodUnknown {
            get {
                return ResourceManager.GetString("Processing_ERROR_Notification_DeliveryMethodUnknown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The test notification was received and ignored..
        /// </summary>
        internal static string Processing_ERROR_Notification_Test {
            get {
                return ResourceManager.GetString("Processing_ERROR_Notification_Test", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to It was not possible to retrieve data necessary to process this notification..
        /// </summary>
        internal static string Processing_ERROR_Scenario_DataNotFound {
            get {
                return ResourceManager.GetString("Processing_ERROR_Scenario_DataNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The notification has not been sent to Notify NL..
        /// </summary>
        internal static string Processing_ERROR_Scenario_NotificationNotSent {
            get {
                return ResourceManager.GetString("Processing_ERROR_Scenario_NotificationNotSent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to It was not possible to determine what to do with the received notification..
        /// </summary>
        internal static string Processing_ERROR_Scenario_NotImplemented {
            get {
                return ResourceManager.GetString("Processing_ERROR_Scenario_NotImplemented", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The notification was sent but the completion status could not be delivered to the external telemetry API endpoint..
        /// </summary>
        internal static string Processing_ERROR_Telemetry_CompletionNotSent {
            get {
                return ResourceManager.GetString("Processing_ERROR_Telemetry_CompletionNotSent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not standardized (unexpected) API response.
        /// </summary>
        internal static string Processing_ERROR_UnspecifiedResponse {
            get {
                return ResourceManager.GetString("Processing_ERROR_UnspecifiedResponse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The notification has been successfully processed and sent to Notify NL..
        /// </summary>
        internal static string Processing_SUCCESS_Scenario_NotificationSent {
            get {
                return ResourceManager.GetString("Processing_SUCCESS_Scenario_NotificationSent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The notification was passed to NotifyNL API..
        /// </summary>
        internal static string Register_NotifyNL_SUCCESS_NotificationSent {
            get {
                return ResourceManager.GetString("Register_NotifyNL_SUCCESS_NotificationSent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Insert received JWT token here.
        /// </summary>
        internal static string Swagger_Authentication_Description {
            get {
                return ResourceManager.GetString("Swagger_Authentication_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to API handling the data and communication workflow between multiple third-party components in order to send notifications through Notify NL..
        /// </summary>
        internal static string Swagger_Description {
            get {
                return ResourceManager.GetString("Swagger_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OMC (Output Management Component).
        /// </summary>
        internal static string Swagger_Title {
            get {
                return ResourceManager.GetString("Swagger_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1.0.
        /// </summary>
        internal static string Swagger_Version {
            get {
                return ResourceManager.GetString("Swagger_Version", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This notification method is not supported..
        /// </summary>
        internal static string Test_NotifyNL_ERROR_NotSupportedMethod {
            get {
                return ResourceManager.GetString("Test_NotifyNL_ERROR_NotSupportedMethod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to was successfully send to NotifyNL..
        /// </summary>
        internal static string Test_NotifyNL_SUCCESS_NotificationSent {
            get {
                return ResourceManager.GetString("Test_NotifyNL_SUCCESS_NotificationSent", resourceCulture);
            }
        }
    }
}
