﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebQueries.Properties {
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
    public class QueryResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal QueryResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WebQueries.Properties.QueryResources", typeof(QueryResources).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This type of HttpClient is not supported yet:.
        /// </summary>
        public static string Authorization_ERROR_HttpClientTypeNotSuported {
            get {
                return ResourceManager.GetString("Authorization_ERROR_HttpClientTypeNotSuported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Internal error: The BSN number is expected..
        /// </summary>
        public static string Querying_ERROR_Internal_MissingBsnNumber {
            get {
                return ResourceManager.GetString("Querying_ERROR_Internal_MissingBsnNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Internal error: The given URI is not a case type..
        /// </summary>
        public static string Querying_ERROR_Internal_NotCaseTypeUri {
            get {
                return ResourceManager.GetString("Querying_ERROR_Internal_NotCaseTypeUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Internal error: The given URI is not a case..
        /// </summary>
        public static string Querying_ERROR_Internal_NotCaseUri {
            get {
                return ResourceManager.GetString("Querying_ERROR_Internal_NotCaseUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Internal error: The given URI is not a decision resource..
        /// </summary>
        public static string Querying_ERROR_Internal_NotDecisionResourceUri {
            get {
                return ResourceManager.GetString("Querying_ERROR_Internal_NotDecisionResourceUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Internal error: The given URI is not an information object..
        /// </summary>
        public static string Querying_ERROR_Internal_NotInfoObjectUri {
            get {
                return ResourceManager.GetString("Querying_ERROR_Internal_NotInfoObjectUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Internal error: The given URI is not an object..
        /// </summary>
        public static string Querying_ERROR_Internal_NotObjectUri {
            get {
                return ResourceManager.GetString("Querying_ERROR_Internal_NotObjectUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Internal error: The given URI is not a party..
        /// </summary>
        public static string Querying_ERROR_Internal_NotPartyUri {
            get {
                return ResourceManager.GetString("Querying_ERROR_Internal_NotPartyUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The registration of details about successfully processed notification was completed..
        /// </summary>
        public static string Registering_SUCCESS_NotificationSentToNotifyNL {
            get {
                return ResourceManager.GetString("Registering_SUCCESS_NotificationSentToNotifyNL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The notification method (&apos;aanmaakkanaal&apos;) is unknown. The citizen data are incomplete. There should be declared at least one distribution channel: SMS, e-mail, Both, or None..
        /// </summary>
        public static string Response_ProcessingData_ERROR_DeliveryMethodUnknown {
            get {
                return ResourceManager.GetString("Response_ProcessingData_ERROR_DeliveryMethodUnknown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The notification method is missing, unknown, or not supported..
        /// </summary>
        public static string Response_QueryingData_ERROR_NotificationMethodMissing {
            get {
                return ResourceManager.GetString("Response_QueryingData_ERROR_NotificationMethodMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to All the necessary data were retrieved successfully from Web API query services..
        /// </summary>
        public static string Response_QueryingData_SUCCESS_DataRetrieved {
            get {
                return ResourceManager.GetString("Response_QueryingData_SUCCESS_DataRetrieved", resourceCulture);
            }
        }
    }
}
