﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SafeMessenge.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SafeMessenge.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to SELECT
        ///    Users.Id AS [Id],
        ///    Users.UserName AS [Name],
        ///    Users.Password AS [Password],
        ///    Users.IsAdmin AS [IsAdmin],
        ///    Users.ClearanceId AS [ClearanceId],
        ///    PaswordTypes.Id AS [PasswordTypeId],
        ///    PaswordTypes.Name AS [PasswordTypeName],
        ///    PaswordTypes.ValidationRegex AS [PasswordValidationRegex],
        ///    PaswordTypes.Description AS [PasswordTypeDescription]
        ///FROM Users
        ///LEFT JOIN PaswordTypes ON PaswordTypes.Id = PasswordTypeId.
        /// </summary>
        internal static string GetAllUsers {
            get {
                return ResourceManager.GetString("GetAllUsers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Id, Name, ValidationRegex, Description FROM PaswordTypes.
        /// </summary>
        internal static string GetPasswordTypes {
            get {
                return ResourceManager.GetString("GetPasswordTypes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    SC.Id AS Id,
        ///    SC.Lvl AS Lvl,
        ///    SC.Name AS Name,
        ///    SC.ActionTypeId AS ActionTypeId,
        ///    AT.IsReadAble AS IsReadAble,
        ///    AT.IsWriteAble AS IsWriteAble,
        ///    AT.Name AS ActionTypeName
        ///FROM SecurityClearances AS SC
        ///LEFT JOIN ActionTypes AS AT ON AT.Id = SC.ActionTypeId.
        /// </summary>
        internal static string GetSecurityClearances {
            get {
                return ResourceManager.GetString("GetSecurityClearances", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    Users.Id AS [Id],
        ///    Users.UserName AS [Name],
        ///    Users.Password AS [Password],
        ///    Users.IsAdmin AS [IsAdmin],
        ///    Users.ClearanceId AS [ClearanceId],
        ///    PaswordTypes.Id AS [PasswordTypeId],
        ///    PaswordTypes.Name AS [PasswordTypeName],
        ///    PaswordTypes.ValidationRegex AS [PasswordValidationRegex],
        ///    PaswordTypes.Description AS [PasswordTypeDescription]
        ///FROM Users
        ///LEFT JOIN PaswordTypes ON PaswordTypes.Id = PasswordTypeId
        ///WHERE Users.Id = @Id;.
        /// </summary>
        internal static string GetUserById {
            get {
                return ResourceManager.GetString("GetUserById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    F.Id AS Id,
        ///    F.Name AS Name,
        ///    F.FilePath AS FilePath,
        ///    SC.Name AS ClearanceName,
        ///    AT.IsReadAble AS IsReadAble,
        ///    AT.IsWriteAble AS IsWriteAble
        ///FROM Files F
        ///LEFT JOIN SecurityClearances AS SC ON SC.Id = F.MinimumClearanceId
        ///LEFT JOIN ActionTypes AS AT ON AT.Id = SC.ActionTypeId
        ///WHERE SC.Lvl &lt;= (
        ///                    SELECT
        ///                        MAX(USC.Lvl) AS Lvl
        ///                    FROM Users AS U
        ///                    LEFT JOIN SecurityClearances AS USC ON USC.Id = U. [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GetUserFilesByUserId {
            get {
                return ResourceManager.GetString("GetUserFilesByUserId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP TABLE IF EXISTS [PaswordTypes];
        ///CREATE TABLE IF NOT EXISTS [PaswordTypes] (
        ///  [Id] INTEGER PRIMARY KEY AUTOINCREMENT
        ///, [Name] NVARCHAR(64) NOT NULL
        ///, [ValidationRegex] TEXT NOT NULL
        ///, [Description] TEXT NOT NULL
        ///, [Created] DATETIME default current_timestamp
        ///);
        ///INSERT OR IGNORE INTO PaswordTypes ([Id],[Name], [ValidationRegex], [Description]) VALUES
        ///(1, &apos;Простий&apos;, &apos;.&apos;, &apos;Пароль повивнен містити мінімум один символ&apos;),
        ///(2, &apos;Складний&apos;, &apos;^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$&apos;, &apos;Пароль має [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string InsertTables {
            get {
                return ResourceManager.GetString("InsertTables", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO Users ([UserName], [Password], [PasswordTypeId], [IsAdmin]) VALUES
        ///(@UserName, &apos;&apos;, @PasswordTypeId, @IsAdmin);
        ///SELECT
        ///    Users.Id AS [Id],
        ///    Users.UserName AS [Name],
        ///    Users.Password AS [Password],
        ///    Users.IsAdmin AS [IsAdmin],
        ///    Users.ClearanceId AS [ClearanceId],
        ///    PaswordTypes.Id AS [PasswordTypeId],
        ///    PaswordTypes.Name AS [PasswordTypeName],
        ///    PaswordTypes.ValidationRegex AS [PasswordValidationRegex],
        ///    PaswordTypes.Description AS [PasswordTypeDescription]
        ///FROM  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string InsertUser {
            get {
                return ResourceManager.GetString("InsertUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE [Users] SET
        ///[UserName] = @UserName,
        ///[Password] = @Password,
        ///[IsAdmin] = @IsAdmin,
        ///[PasswordTypeId] = @PasswordTypeId,
        ///[ClearanceId] = @ClearanceId
        ///WHERE [Id] = @Id;
        ///.
        /// </summary>
        internal static string UpdateUser {
            get {
                return ResourceManager.GetString("UpdateUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE [Users] SET
        ///[Password] = @Password,
        ///WHERE [Id] = @Id;
        ///.
        /// </summary>
        internal static string UpdateUserPassword {
            get {
                return ResourceManager.GetString("UpdateUserPassword", resourceCulture);
            }
        }
    }
}
