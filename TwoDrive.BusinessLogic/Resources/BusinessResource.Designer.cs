﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TwoDrive.BusinessLogic.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class BusinessResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal BusinessResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TwoDrive.BusinessLogic.Resources.BusinessResource", typeof(BusinessResource).Assembly);
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
        ///   Looks up a localized string similar to {0} can already {1} this element.
        /// </summary>
        public static string AlreadyHas_Claims {
            get {
                return ResourceManager.GetString("AlreadyHas_Claims", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can&apos;t add creator claims to root.
        /// </summary>
        public static string CantAddCreatorClaims {
            get {
                return ResourceManager.GetString("CantAddCreatorClaims", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Destination is child of element to move.
        /// </summary>
        public static string ChildDestination_FolderValidator {
            get {
                return ResourceManager.GetString("ChildDestination_FolderValidator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The modified date should be later than the creation date.
        /// </summary>
        public static string Date_ElementValidator {
            get {
                return ResourceManager.GetString("Date_ElementValidator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Destination doesn&apos;t exists.
        /// </summary>
        public static string DestinationNotFound_FolderValidator {
            get {
                return ResourceManager.GetString("DestinationNotFound_FolderValidator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Destination must be my root&apos;s child.
        /// </summary>
        public static string DestinationNotInRoot_FolderValidator {
            get {
                return ResourceManager.GetString("DestinationNotInRoot_FolderValidator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to End date cannot be earlier than start date.
        /// </summary>
        public static string EndBeforeStart {
            get {
                return ResourceManager.GetString("EndBeforeStart", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Writer already has creator claims for this element.
        /// </summary>
        public static string ExisitngCreatorClaims_Claims {
            get {
                return ResourceManager.GetString("ExisitngCreatorClaims_Claims", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Folder not found.
        /// </summary>
        public static string FolderNotFound {
            get {
                return ResourceManager.GetString("FolderNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dependencies must be set to validate destination.
        /// </summary>
        public static string MissingDependencies_FolderValidator {
            get {
                return ResourceManager.GetString("MissingDependencies_FolderValidator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A file should have a parent folder.
        /// </summary>
        public static string MissingParent_FileValidator {
            get {
                return ResourceManager.GetString("MissingParent_FileValidator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A child folder must have a parent folder.
        /// </summary>
        public static string MissingParent_FolderValidator {
            get {
                return ResourceManager.GetString("MissingParent_FolderValidator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Writer needs a password set.
        /// </summary>
        public static string MissingPassword_WriterValidator {
            get {
                return ResourceManager.GetString("MissingPassword_WriterValidator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Writer needs an username .
        /// </summary>
        public static string MissingUserName_WriterValidator {
            get {
                return ResourceManager.GetString("MissingUserName_WriterValidator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Folder or File must have a owner.
        /// </summary>
        public static string MustHaveAnOwner_ElementValidator {
            get {
                return ResourceManager.GetString("MustHaveAnOwner_ElementValidator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Folder or File must have a name.
        /// </summary>
        public static string Name_ElementValidator {
            get {
                return ResourceManager.GetString("Name_ElementValidator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No claims to remove.
        /// </summary>
        public static string NoClaims {
            get {
                return ResourceManager.GetString("NoClaims", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The owner is not friends with {0}.
        /// </summary>
        public static string NotFriends {
            get {
                return ResourceManager.GetString("NotFriends", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can&apos;t be null.
        /// </summary>
        public static string NotNull_Validator {
            get {
                return ResourceManager.GetString("NotNull_Validator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} is not owner of this element.
        /// </summary>
        public static string NotOwner {
            get {
                return ResourceManager.GetString("NotOwner", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The username must be unique. Try another username.
        /// </summary>
        public static string NotUnique_WriterValidator {
            get {
                return ResourceManager.GetString("NotUnique_WriterValidator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can&apos;t add root claims to a child folder.
        /// </summary>
        public static string RootClaims_Claims {
            get {
                return ResourceManager.GetString("RootClaims_Claims", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Two files at same level can have the same name.
        /// </summary>
        public static string SameName_FileValidator {
            get {
                return ResourceManager.GetString("SameName_FileValidator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Two folders at same level can have the same name.
        /// </summary>
        public static string SameName_FolderValidator {
            get {
                return ResourceManager.GetString("SameName_FolderValidator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} +- {1} \n.
        /// </summary>
        public static string ShowTreeFormat {
            get {
                return ResourceManager.GetString("ShowTreeFormat", resourceCulture);
            }
        }
    }
}
