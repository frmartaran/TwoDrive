﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TwoDrive.WebApi.Resource {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ApiResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ApiResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TwoDrive.WebApi.Resource.ApiResource", typeof(ApiResource).Assembly);
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
        ///   Looks up a localized string similar to You&apos;re already friend with {0}.
        /// </summary>
        public static string AlreadyFriends {
            get {
                return ResourceManager.GetString("AlreadyFriends", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can&apos;t remove friend since you aren&apos;t friends.
        /// </summary>
        public static string CantRemove {
            get {
                return ResourceManager.GetString("CantRemove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Writer Created.
        /// </summary>
        public static string Created_WriterController {
            get {
                return ResourceManager.GetString("Created_WriterController", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File {0} has been deleted.
        /// </summary>
        public static string Delete_FileController {
            get {
                return ResourceManager.GetString("Delete_FileController", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Folder: {0} has been deleted.
        /// </summary>
        public static string Deleted_FolderController {
            get {
                return ResourceManager.GetString("Deleted_FolderController", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Writer: {0} has been deleted.
        /// </summary>
        public static string Deleted_WriterController {
            get {
                return ResourceManager.GetString("Deleted_WriterController", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Element not found.
        /// </summary>
        public static string ElementNotFound_ClaimFilter {
            get {
                return ResourceManager.GetString("ElementNotFound_ClaimFilter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File Not Found.
        /// </summary>
        public static string FileNotFound {
            get {
                return ResourceManager.GetString("FileNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No files found.
        /// </summary>
        public static string FilesNotFound {
            get {
                return ResourceManager.GetString("FilesNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        public static string FileUpdated {
            get {
                return ResourceManager.GetString("FileUpdated", resourceCulture);
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
        ///   Looks up a localized string similar to A writer can&apos;t be his own friend.
        /// </summary>
        public static string FriendAndWriterAreTheSame {
            get {
                return ResourceManager.GetString("FriendAndWriterAreTheSame", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Friend Not Found.
        /// </summary>
        public static string FriendNotFound {
            get {
                return ResourceManager.GetString("FriendNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tree imported successfully.
        /// </summary>
        public static string Import_Success {
            get {
                return ResourceManager.GetString("Import_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid Token.
        /// </summary>
        public static string InvalidToken {
            get {
                return ResourceManager.GetString("InvalidToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Incorrect username or password.
        /// </summary>
        public static string LoginError_TokenController {
            get {
                return ResourceManager.GetString("LoginError_TokenController", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bye!.
        /// </summary>
        public static string LogOut_TokenController {
            get {
                return ResourceManager.GetString("LogOut_TokenController", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was an error logging out.
        /// </summary>
        public static string LogOutError_TokenController {
            get {
                return ResourceManager.GetString("LogOutError_TokenController", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Token is required.
        /// </summary>
        public static string MissingToken {
            get {
                return ResourceManager.GetString("MissingToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File: {0} moved to {1}.
        /// </summary>
        public static string Moved_FileController {
            get {
                return ResourceManager.GetString("Moved_FileController", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Folder {0} was moved to destination {1}.
        /// </summary>
        public static string Moved_FolderController {
            get {
                return ResourceManager.GetString("Moved_FolderController", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You must be friends with {0}.
        /// </summary>
        public static string MustBeFriends {
            get {
                return ResourceManager.GetString("MustBeFriends", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Must Log In First.
        /// </summary>
        public static string MustLogIn {
            get {
                return ResourceManager.GetString("MustLogIn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You must own both elements.
        /// </summary>
        public static string MustOwn_FolderController {
            get {
                return ResourceManager.GetString("MustOwn_FolderController", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Writer has no friends.
        /// </summary>
        public static string NoFriends {
            get {
                return ResourceManager.GetString("NoFriends", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There haven&apos;t been any modfications yet.
        /// </summary>
        public static string NoModificationsYet_ReportController {
            get {
                return ResourceManager.GetString("NoModificationsYet_ReportController", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The user {0} is not allowed to {1} this element.
        /// </summary>
        public static string NotAllowed_ClaimFilter {
            get {
                return ResourceManager.GetString("NotAllowed_ClaimFilter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User not authorized.
        /// </summary>
        public static string NotAuthorized_AuthorizeFilter {
            get {
                return ResourceManager.GetString("NotAuthorized_AuthorizeFilter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are not friends with {0}.
        /// </summary>
        public static string NotFriends {
            get {
                return ResourceManager.GetString("NotFriends", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are no top writers yet.
        /// </summary>
        public static string NoTopWriters_ReportController {
            get {
                return ResourceManager.GetString("NoTopWriters_ReportController", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not owner of {0}.
        /// </summary>
        public static string NotOwnerOf {
            get {
                return ResourceManager.GetString("NotOwnerOf", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are now friends with {0}.
        /// </summary>
        public static string NowFriends {
            get {
                return ResourceManager.GetString("NowFriends", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are not friends with {0} anymore.
        /// </summary>
        public static string NowNotFriends {
            get {
                return ResourceManager.GetString("NowNotFriends", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parent Folder Not Found.
        /// </summary>
        public static string ParentFolderNotFound {
            get {
                return ResourceManager.GetString("ParentFolderNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Json reader threw an exception.
        /// </summary>
        public static string Reader_JSON {
            get {
                return ResourceManager.GetString("Reader_JSON", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Json serializer threw an exception.
        /// </summary>
        public static string Serializer_JSON {
            get {
                return ResourceManager.GetString("Serializer_JSON", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} has been shared with {1}.
        /// </summary>
        public static string Shared {
            get {
                return ResourceManager.GetString("Shared", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stopped sharing {0} with {1}.
        /// </summary>
        public static string StopSharing {
            get {
                return ResourceManager.GetString("StopSharing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsupported type of element, can&apos;t get model.
        /// </summary>
        public static string UnsupportedElementType {
            get {
                return ResourceManager.GetString("UnsupportedElementType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsupported type of file, can&apos;t get model.
        /// </summary>
        public static string UnsupportedFileType {
            get {
                return ResourceManager.GetString("UnsupportedFileType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Writer not found.
        /// </summary>
        public static string WriterNotFound {
            get {
                return ResourceManager.GetString("WriterNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to no writers found.
        /// </summary>
        public static string WritersNotFound {
            get {
                return ResourceManager.GetString("WritersNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Something&apos;s went wrong in the request body.
        /// </summary>
        public static string WrongRequestBody {
            get {
                return ResourceManager.GetString("WrongRequestBody", resourceCulture);
            }
        }
    }
}
