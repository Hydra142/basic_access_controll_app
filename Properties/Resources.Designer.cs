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
        ///   Looks up a localized string similar to DELETE FROM DiscretionaryAccessMatrix WHERE Id IN @Ids.
        /// </summary>
        internal static string DeleteDiscretionaryAccessMatrixItems {
            get {
                return ResourceManager.GetString("DeleteDiscretionaryAccessMatrixItems", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM RoleFiles WHERE Id IN @Ids.
        /// </summary>
        internal static string DeleteRoleFiles {
            get {
                return ResourceManager.GetString("DeleteRoleFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM UserRoles WHERE Id IN @Ids.
        /// </summary>
        internal static string DeleteUserRoles {
            get {
                return ResourceManager.GetString("DeleteUserRoles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Id, IsReadAble, IsWriteAble, IsExecuteAble, Name FROM ActionTypes.
        /// </summary>
        internal static string GetActionTypes {
            get {
                return ResourceManager.GetString("GetActionTypes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///	Id,
        ///	Name,
        ///	FilePath,
        ///	FileType,
        ///	MinimumClearanceId
        ///FROM Files.
        /// </summary>
        internal static string GetAllFiles {
            get {
                return ResourceManager.GetString("GetAllFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM ROLES;.
        /// </summary>
        internal static string GetAllRoles {
            get {
                return ResourceManager.GetString("GetAllRoles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    Users.Id AS [Id],
        ///    Users.UserName AS [Name],
        ///    Users.Password AS [Password],
        ///    Users.IsAdmin AS [IsAdmin],
        ///    Users.ClearanceId AS [ClearanceId],
        ///    Users.ActionTypeId AS [ActionTypeId],
        ///    Users.AccessControlModelId AS [AccessControlModelId],
        ///    PaswordTypes.Id AS [PasswordTypeId],
        ///    PaswordTypes.Name AS [PasswordTypeName],
        ///    PaswordTypes.ValidationRegex AS [PasswordValidationRegex],
        ///    PaswordTypes.Description AS [PasswordTypeDescription]
        ///FROM Users
        ///LEFT JOIN Paswor [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GetAllUsers {
            get {
                return ResourceManager.GetString("GetAllUsers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    F.Id AS Id,
        ///    F.Name AS Name,
        ///    F.FilePath AS FilePath,
        ///    F.FileType AS FileType,
        ///    AT.Name AS ActionTypeName,
        ///    AT.IsReadAble AS IsReadAble,
        ///    AT.IsWriteAble AS IsWriteAble,
        ///    AT.IsExecuteAble AS IsExecuteAble,
        ///    RF.AllowFrom AS AllowFrom,
        ///    RF.AllowTo AS AllowTo,
        ///    CASE
        ///        WHEN (substr(AllowFrom, 12, 8) = &apos;00:00:00&apos; AND substr(AllowTo, 12, 8) = &apos;00:00:00&apos;) THEN &apos;час не обмежений&apos;
        ///        ELSE (substr(AllowFrom, 12, 5) || &apos; - &apos; || substr(AllowTo, 12, 5))
        ///   [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GetAvailableUserFiles {
            get {
                return ResourceManager.GetString("GetAvailableUserFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    F.Id AS Id,
        ///    F.Name AS Name,
        ///    F.FilePath AS FilePath,
        ///    F.FileType AS FileType,
        ///    AT.Name AS ActionTypeName,
        ///    AT.IsReadAble AS IsReadAble,
        ///    AT.IsWriteAble AS IsWriteAble,
        ///    AT.IsExecuteAble AS IsExecuteAble,
        ///    DAM.AllowFrom AS AllowFrom,
        ///    DAM.AllowTo AS AllowTo,
        ///    CASE
        ///        WHEN (substr(AllowFrom, 12, 8) = &apos;00:00:00&apos; AND substr(AllowTo, 12, 8) = &apos;00:00:00&apos;) THEN &apos;час не обмежений&apos;
        ///        ELSE (substr(AllowFrom, 12, 5) || &apos; - &apos; || substr(AllowTo, 12, 5))
        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GetDiscretionaryAccessModelUserAvailableFilesById {
            get {
                return ResourceManager.GetString("GetDiscretionaryAccessModelUserAvailableFilesById", resourceCulture);
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
        ///	M.Id AS Id,
        ///	R.Id AS RoleId,
        ///	F.Id AS FileId,
        ///	F.Name AS FileName,
        ///	IFNULL(AT.Id, 1) AS ActionTypeId,
        ///	M.AllowFrom AS AllowFrom,
        ///	M.AllowTo AS AllowTo,
        ///	M.Id AS IsActive /*перевірка чи активне дане правило*/
        ///FROM Files AS F
        ///LEFT JOIN Roles AS R ON R.Id = @RoleId
        ///LEFT JOIN RoleFiles AS M ON M.RoleId = R.Id AND M.FileId = F.Id
        ///LEFT JOIN ActionTypes AS AT ON AT.Id = M.ActionTypeId
        ///WHERE R.Id = @RoleId
        ///ORDER BY F.Name.
        /// </summary>
        internal static string GetRoleFiles {
            get {
                return ResourceManager.GetString("GetRoleFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    SC.Id AS Id,
        ///    SC.Lvl AS Lvl,
        ///    SC.Name AS Name
        ///FROM SecurityClearances AS SC.
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
        ///    Users.ActionTypeId AS [ActionTypeId],
        ///    Users.AccessControlModelId AS [AccessControlModelId],
        ///    PaswordTypes.Id AS [PasswordTypeId],
        ///    PaswordTypes.Name AS [PasswordTypeName],
        ///    PaswordTypes.ValidationRegex AS [PasswordValidationRegex],
        ///    PaswordTypes.Description AS [PasswordTypeDescription]
        ///FROM Users
        ///LEFT JOIN Paswor [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GetUserById {
            get {
                return ResourceManager.GetString("GetUserById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///	M.Id AS Id,
        ///	U.Id AS UserId,
        ///	F.Id AS FileId,
        ///	F.Name AS FileName,
        ///	IFNULL(AT.Id, 1) AS ActionTypeId,
        ///	M.AllowFrom AS AllowFrom,
        ///	M.AllowTo AS AllowTo,
        ///	M.Id AS IsActive /*перевірка чи активне дане правило*/
        ///FROM Files AS F
        ///LEFT JOIN Users AS U ON U.Id = @UserId
        ///LEFT JOIN DiscretionaryAccessMatrix AS M ON M.UserId = U.Id AND M.FileId = F.Id
        ///LEFT JOIN ActionTypes AS AT ON AT.Id = M.ActionTypeId
        ///WHERE U.Id = @UserId
        ///ORDER BY F.Name.
        /// </summary>
        internal static string GetUserDiscretionaryAccessMatrixById {
            get {
                return ResourceManager.GetString("GetUserDiscretionaryAccessMatrixById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    F.Id AS Id,
        ///    F.Name AS Name, /*Назва файлу*/
        ///    F.FilePath AS FilePath,
        ///    F.FileType AS FileType,
        ///    SC.Name AS ClearanceName,/*Назва рівня доступу*/
        ///    AT.Name AS ActionTypeName,
        ///    AT.IsReadAble AS IsReadAble, /*дозволені дії*/
        ///    AT.IsWriteAble AS IsWriteAble,
        ///    AT.IsExecuteAble AS IsExecuteAble
        ////*Вибір всіх файлів з таблиці*/
        ///FROM Files F
        ////*Створюємо всі можливі комбідації файл - користувач*/
        ///CROSS JOIN Users AS U
        ////*приєднуємо відповідний рівень доступу цього файлу*/ [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GetUserFilesByUserId {
            get {
                return ResourceManager.GetString("GetUserFilesByUserId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///	UR.Id AS Id,
        ///	R.Id AS RoleId,
        ///	R.Name AS RoleName,
        ///	U.Id AS UserId,
        ///	UR.Id AS IsActive /*перевірка чи активне дане правило*/
        ///FROM Roles AS R
        ///LEFT JOIN Users AS U ON U.Id = @UserId
        ///LEFT JOIN UserRoles AS UR ON UR.RoleId = R.Id AND UR.UserId = U.Id
        ///WHERE U.Id = @UserId
        ///ORDER BY R.Id.
        /// </summary>
        internal static string GetUserRoles {
            get {
                return ResourceManager.GetString("GetUserRoles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT OR REPLACE INTO DiscretionaryAccessMatrix (Id, UserId, FileId, ActionTypeId, AllowFrom, AllowTo)
        ///VALUES (@Id, @UserId, @FileId, @ActionTypeId, @AllowFrom, @AllowTo);
        ///.
        /// </summary>
        internal static string InsertOrUpdateDiscretionaryAccessMatrixItem {
            get {
                return ResourceManager.GetString("InsertOrUpdateDiscretionaryAccessMatrixItem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT OR REPLACE INTO Files (Id, Name, FilePath, FileType, MinimumClearanceId)
        ///VALUES (@Id, @Name, @FilePath, @FileType, @MinimumClearanceId);
        ///SELECT
        ///	Id,
        ///	Name,
        ///	FilePath,
        ///	FileType,
        ///	MinimumClearanceId
        ///FROM Files WHERE Id = last_insert_rowid();
        ////*ON DUPLICATE KEY UPDATE Name = @Name, FilePath = @FilePath, FileType = @FileType, MinimumClearanceId =@MinimumClearanceId*/.
        /// </summary>
        internal static string InsertOrUpdateFile {
            get {
                return ResourceManager.GetString("InsertOrUpdateFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT OR REPLACE INTO Roles (Id, Name)
        ///VALUES (@Id, @Name);
        ///SELECT
        ///	*
        ///FROM Roles WHERE Id = last_insert_rowid();
        ///.
        /// </summary>
        internal static string InsertOrUpdateRole {
            get {
                return ResourceManager.GetString("InsertOrUpdateRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT OR REPLACE INTO RoleFiles (Id, RoleId, FileId, ActionTypeId, AllowFrom, AllowTo)
        ///VALUES (@Id, @RoleId, @FileId, @ActionTypeId, @AllowFrom, @AllowTo);
        ///.
        /// </summary>
        internal static string InsertOrUpdateRoleFile {
            get {
                return ResourceManager.GetString("InsertOrUpdateRoleFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT OR REPLACE INTO UserRoles (Id, RoleId, UserId)
        ///VALUES (@Id, @RoleId, @UserId);
        ///.
        /// </summary>
        internal static string InsertOrUpdateUserRole {
            get {
                return ResourceManager.GetString("InsertOrUpdateUserRole", resourceCulture);
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
        ///    Users.AccessControlModelId AS [AccessControlModelId],
        ///    PaswordTypes.Id AS [PasswordTypeId],
        ///    PaswordTypes.Name AS [PasswordTypeName],
        ///    PaswordTypes.ValidationRegex AS [PasswordValidationRegex],
        ///    P [rest of string was truncated]&quot;;.
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
        ///[ClearanceId] = @ClearanceId,
        ///[ActionTypeId] = @ActionTypeId,
        ///[AccessControlModelId] = @AccessControlModelId
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
        ///[Password] = @Password
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
