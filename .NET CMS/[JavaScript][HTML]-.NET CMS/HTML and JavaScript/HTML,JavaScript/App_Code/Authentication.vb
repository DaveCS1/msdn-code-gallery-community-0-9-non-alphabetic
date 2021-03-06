'© By Andrea Bruno
'Open source, but: This source code (or part of this code) is not usable in other applications
Option Explicit On
Option Strict On

Imports Microsoft.VisualBasic

Namespace WebApplication
	Public Module Authentication
		'configuration
    Private UsersDirectory As String = UsersSubDirectory

		Public Class User
			Inherits Common.Attributes
			Shared Function Load(ByVal UserName As String) As Authentication.User
				Try
					UserName = LCase(UserName)
					If UserName <> "" Then
						If Not CacheUsers.ContainsKey(UserName) Then
							'Load from file
							Dim NewUser As User = LoadFromFile(UserName)
							If NewUser IsNot Nothing Then
								If Not CacheUsers.ContainsKey(UserName) Then
									CacheUsers.Add(UserName, NewUser)
								End If
								Return CacheUsers(UserName)
							End If
						Else
							'Quick load
							Return CacheUsers(UserName)
						End If
					End If
				Catch ex As Exception
					Log("ErrorUserLoad", 1000, UserName, ex.Source, ex.Message)
				End Try
        Return Nothing
      End Function

      Function Href(ByVal Setting As SubSite) As String
        Return Common.Href(Setting.Name, False, "homeuser.aspx", "u", Me.Username)
      End Function

			Function AvatarControl(ByVal Setting As SubSite) As Control
        Dim Cache As Collection = CType(HttpContext.Current.Items("CacheUserAvatarControl"), Collection)
        Dim Key As String = Username & "." & Setting.Name
        If Cache IsNot Nothing Then
          If Cache.Contains(Key) Then
            Return CType(Cache(Key), Control)
          End If
        Else
          Cache = New Collection
          HttpContext.Current.Items("CacheUserAvatarControl") = Cache
        End If
        If PhotoAlbum() IsNot Nothing Then
          Dim Photos As Integer() = AllPhotosName(PhotoAlbum.Name)
          If Photos IsNot Nothing AndAlso Photos.Length > 0 Then
            Dim Photo As Photo = PhotoManager.Photo.Load(Photos(0), PhotoAlbum.Name)
            Dim Control As Control = Photo.ControlThumbnail(Setting, True, , ThumbnailBoxMode.Avatar)
            Cache.Add(Control, Key)
            Return Control
          End If
        End If
        Cache.Add(Nothing, Key)
        Return Nothing
			End Function

			Private Shared Function LoadFromFile(ByVal UserName As String) As User
				If IO.File.Exists(FileName(UserName)) Then
          Dim User As User = CType(Deserialize(FileName(UserName), GetType(User)), Authentication.User)
					If User IsNot Nothing Then
						Return User
          End If
        End If
        Return Nothing
			End Function

			Sub Save()
				If Username IsNot Nothing Then
					If Not CacheUsers.ContainsKey(LCase(Me.Username)) Then
						CacheUsers.Add(LCase(Me.Username), Me)
					End If
					If FirstAccess = New Date Then
						'Is the first access!
						FirstAccess = Now.ToUniversalTime
					End If
					Serialize(Me, FileName(Username))
				End If
			End Sub

			Private Shared Function FileName(ByVal Username As String) As String
				Return MapPath(UsersDirectory & "/" & AbjustNameFile(FirstUpper(Username)) & ".xml")
			End Function

			Public StartTime As Date = Now.ToUniversalTime
			Public FirstAccess As Date
			Public LastAccess As Date
			Public FirstMessageInForum As Date
			Public IP As String	'IP user in the last access
			Public ProxyUse As Boolean
			Public CodeUser As String
			Public Forwarded As String
			Public Identity As String
			Public Generate As String	'Not nothing if this user is generated from another user
			Public DomainName As String	'UserDomainName in the last access
			Public SubSite As String 'Subsite in the last access
			Public AutoLogIn As Boolean = True
			Private Name As String '= "" 'Set a Non-Nothing ("") value for not generate error with StrComp operation
			Property Username() As String
				Get
					Return Name
				End Get
				Set(ByVal Value As String)
					Name = FirstUpper(Value)
				End Set
			End Property
			Public Password As String
			Public FirstName As String
			Public LastName As String
			Public Phone As String
			Public Country As String
			Public GMT As Integer = +1
			Public TimeOffsetSeconds As Long
			Private TimeOffsetDetected As Boolean
			Sub DetectTimeOffsetDisabled()
				TimeOffsetDetected = True
			End Sub
			Function ForceDetectTimeOffset() As Boolean
				Return Not TimeOffsetDetected
			End Function
			Public City As String
			Public URL As String
			Public Gender As GenderType
			Public Language As LanguageManager.Language = Language.English
			Public AutoForumSubscription As Boolean = True
			Public Enum GenderType As Integer
				NotDefinite
				Male
				Female
			End Enum
			Public Email As String
			Public Skype As String
			Public Enum RoleType
				Excluded = -1
				Visitors = 0
				User = 1
				Senior = 2
				AdministratorJunior = 3
				Administrator = 4
				WebMaster = 5
				Supervisor = 6
			End Enum
			Public GeneralRole As RoleType
			Private Roles As New System.Collections.Generic.Dictionary(Of String, RoleType)
			Public Property RolesArray As RoleElement()
				Get
					'Use to serializze collection Roles
					'Dim I As ICollection(Of KeyValuePair(Of String, RoleType)) = Roles
					Dim Array(Roles.Count - 1) As RoleElement
					Dim n As Integer
					'For Each e As Collections.Generic.KeyValuePair(Of String, RoleType) In I
					For Each Key As String In Roles.Keys
						Array(n) = New RoleElement(Key, Roles(Key))
						n += 1
					Next
					Return Array
				End Get
				Set(ByVal value As RoleElement())
					'Use to deserializze collection Roles
					'Dim I As ICollection(Of KeyValuePair(Of String, RoleType)) = Roles
					For Each Element As RoleElement In value
            Roles.Add(Element.Setting, CType(Element.RoleType, RoleType))
					Next
				End Set
      End Property

			Structure RoleElement
				Public Setting As String
        Public RoleType As Integer
        Sub New(ByVal Setting As String, ByVal RoleType As Integer)
          Me.Setting = Setting
          Me.RoleType = RoleType
        End Sub
			End Structure
			Public Property Role(ByVal SubSite As String) As RoleType
				Get
					If SubSite = "" Then
						Return RoleType.Visitors
					Else
						SubSite = LCase(SubSite)
						Dim MyRole As RoleType
						If Roles.ContainsKey(SubSite) Then
							MyRole = Roles(SubSite)
						Else
							MyRole = RoleType.Visitors
						End If
						If GeneralRole > MyRole OrElse GeneralRole < RoleType.Visitors Then
							MyRole = GeneralRole
						End If
						Return MyRole
					End If
				End Get
				Set(ByVal Value As RoleType)
					If SubSite <> "" Then
						SubSite = LCase(SubSite)
						Dim Current As String = CurrentUser.Username
						If Current <> "" Then
							'Log the changment by user
              Log("RoleChangedByUser", 1000, Me.Username, Current, Value.ToString)


							'Eliminare una volta capito come mai utenti comuni avviano questa funzione
							Log("RoleChangedByUserStack", 1000, Me.Username, Current, Stack)


						End If
						If Roles.ContainsKey(SubSite) Then
							Roles(SubSite) = Value
						Else
							Roles.Add(SubSite, Value)
						End If
					End If
				End Set
			End Property
			Public NamePhotoAlbum As String
			Function PhotoAlbum() As PhotoAlbum
				If NamePhotoAlbum <> "" Then
          PhotoAlbum = CType(PhotoManager.PhotoAlbum.Load(NamePhotoAlbum), PhotoManager.PhotoAlbum)
					If PhotoAlbum IsNot Nothing Then
						Return PhotoAlbum
					End If
				End If
        Return Nothing
      End Function
			Public Avatar As String
			Public Honors As Honors
			'Public CurrentSubsite As Config.SubSite
			Public MessageBoard As Integer
			Private Shared CacheUsers As New System.Collections.Generic.Dictionary(Of String, User)
			Public AverageRating As Single
			Function Rating() As Single
				If CollectionUserRating.Count >= 4 Then
					Return AverageRating
				End If
        Return 0.0!
      End Function
			Sub UpdateRating()
				'calcola il rating usando la media ponderata
				'If CollectionUserRating.Count >= 4 Then
				Dim TotalRating As Single
				Dim All As Single
				For Each Key As String In CollectionUserRating.Keys
					Dim FromUser As User = Load(Key)
					Dim RatingFromUser As Single
					If FromUser.Rating <> 0 Then
						RatingFromUser = FromUser.Rating
					Else
						RatingFromUser = FromUser.CalculatingAverageRating
					End If
					TotalRating += CollectionUserRating(Key) * RatingFromUser
					All += RatingFromUser
				Next
				If All <> 0 Then
					AverageRating = TotalRating / All
				End If
				'End If
			End Sub
			Private Function CalculatingAverageRating() As Single
        If CBool(CollectionUserRating.Count) Then
          Dim TotalRating As Single
          For Each Key As String In CollectionUserRating.Keys
            TotalRating += CollectionUserRating(Key)
          Next
          Return TotalRating / CollectionUserRating.Count
        End If
        Return 0.0!
      End Function
			Property UserRating(ByVal FromUser As String) As Integer
				Get
					If CollectionUserRating.ContainsKey(FromUser) Then
						Return CollectionUserRating(FromUser)
					End If
          Return 0
        End Get
				Set(ByVal value As Integer)
					If CollectionUserRating.ContainsKey(FromUser) Then
						CollectionUserRating.Remove(FromUser)
					End If
					CollectionUserRating.Add(FromUser, value)
				End Set
			End Property
			Private CollectionUserRating As New Collections.Generic.Dictionary(Of String, Integer)

			Public Property UserRatingArray As UserRatingElement()
				Get
					'Use to serializze collection CollectionUserRating
					'Dim I As ICollection(Of KeyValuePair(Of String, Integer)) = CollectionUserRating
					Dim Array(CollectionUserRating.Count - 1) As UserRatingElement
					Dim n As Integer
					'For Each e As Collections.Generic.KeyValuePair(Of String, Integer) In I
					For Each Key As String In CollectionUserRating.Keys
						Array(n) = New UserRatingElement(Key, CollectionUserRating(Key))
						n += 1
					Next
					Return Array
				End Get
				Set(ByVal value As UserRatingElement())
					'Use to deserializze collection CollectionUserRating
					'	Dim I As ICollection(Of KeyValuePair(Of String, Integer)) = Roles
					For Each Element As UserRatingElement In value
						CollectionUserRating.Add(Element.User, Element.Rating)
					Next
				End Set
			End Property

			Structure UserRatingElement
				Public User As String
				Public Rating As Integer
				Sub New(ByVal User As String, ByVal Rating As Integer)
					Me.User = User
					Me.Rating = Rating
				End Sub
			End Structure


			Public Sub SetUser()
				If HttpContext.Current IsNot Nothing Then
					Dim Request As System.Web.HttpRequest = HttpContext.Current.Request
					If Name <> "" AndAlso Request.UserHostAddress <> Me.IP Then
						If Not Setup.Security.EnableProxyUsersToInteract Then	'If the user is abled to interact with proxy then the proxy detection is not necessary 
							Me.ProxyUse = IsProxy(Request.UserHostAddress)
						End If
					End If
					Me.IP = Request.UserHostAddress
					Me.Forwarded = Request.ServerVariables("X-Forwarded-For")
					If Me.Forwarded = "" Then Me.Forwarded = Request.ServerVariables("Via")
					Me.Identity = Request.ServerVariables("LOGON_USER ")
					Me.DomainName = Request.Url.Host
				End If
			End Sub
			Friend LastSessionID As String


			Public Sub LogOut()
        LastAccess = Now.ToUniversalTime.AddMinutes(-Setup.Ambient.SessionTimeOut)
				If GeneralRole <= Authentication.User.RoleType.AdministratorJunior Then
					Dim Preferences As New UserPreferences(Me)
					If Preferences.ShowCensored = True Then
						Preferences.ShowCensored = False
						Preferences.Save(Me)
					End If
				End If
				Save()
			End Sub

			Enum LogOutEvent
				SessionEnd = 0
				SessionClear = 1
				ApplicationEnd = 2
			End Enum

			Private Sub WriteReport(ByVal EndTime As Date)
				Static IsWrited As Boolean
				If IsWrited = False Then
					IsWrited = True
					'Try
          If CBool(Attribute("report")) Then
            Dim Report As String = DateToText(StartTime) & vbTab & DateToText(EndTime) & vbCrLf
            WriteToAppend(Report, MapPath(UsersDirectory & "/" & "reports" & "/" & Username & ".txt"))
            If Me.Email <> "" Then
              SendEmail("Report", Report, Me.Email, , False)
            End If
          End If
					'Catch ex As Exception

					'End Try
				End If
			End Sub

			Private CurrentSessions As New Collections.Specialized.StringCollection

			Sub SetOnline(ByVal Session As Web.SessionState.HttpSessionState)
				SyncLock CurrentSessions
					If Not CurrentSessions.Contains(Session.SessionID) Then
						CurrentSessions.Add(Session.SessionID)
					End If
				End SyncLock
			End Sub

			Sub SetOffline(ByVal Session As Web.SessionState.HttpSessionState)
				SyncLock CurrentSessions
					CurrentSessions.Remove(Session.SessionID)
				End SyncLock
			End Sub

			Function IsOnline() As Boolean
				SyncLock CurrentSessions
					Return CurrentSessions.Count <> 0
				End SyncLock
			End Function

			Public SocialConfiguration As New SocialConfiguration

		End Class

		Enum LogActionType
			LogOn
			LogOff
			NewUser
			EditUser
			LostPassword
		End Enum

		Property CookieUserName() As String
			Get
				Return Cookie("UserName")
			End Get
			Set(ByVal Value As String)
				Cookie("UserName") = Value
			End Set
		End Property

		Sub BlockExcludedUser()
			Dim Block As Boolean
			If CurrentUser.GeneralRole = Authentication.User.RoleType.Excluded Then
				Block = True
			Else
				Dim Value = Cookie("excludeduser")
				If Not Value Is Nothing Then
					If Value = "True" Then
						Block = True
					End If
				End If
			End If

			If Block Then
				If CurrentUser.GeneralRole <> User.RoleType.Excluded Then
					CurrentUser.GeneralRole = User.RoleType.Excluded
					CurrentUser.Save()
				End If
				EndResponse()
			End If
		End Sub

		Sub LockUser(ByVal User As User)
			If User.GeneralRole < Authentication.User.RoleType.AdministratorJunior Then
        Common.AddBannedCodeUsers(User.CodeUser)
				User.GeneralRole = Authentication.User.RoleType.Excluded
				User.Save()
				BlockIP(User.IP, New TimeSpan(7, 0, 0, 0)) 'Block ip for one week
			End If
		End Sub

		Sub RatingUser(ByVal User As User, ByVal Value As String)
			If CurrentUser.GeneralRole >= Authentication.User.RoleType.User Then
				User.UserRating(CurrentUser.Username) = CInt(Value)
				User.UpdateRating()
				User.Save()
			End If
		End Sub

		Public Function ValidateUser(ByVal UserName As String, ByVal password As String) As Boolean
			Dim User As User = User.Load(UserName)
			If IsNothing(User) Then
				Return False
			Else
				Return 0 = StrComp(password, User.Password, CompareMethod.Text)
			End If
		End Function

		Public Function IsValidUsername(ByVal Text As String) As Boolean
			If Trim(Text) = "" Then
				IsValidUsername = False
			ElseIf Text.Chars(0) = " " Or Text.Chars(Text.Length - 1) = " " Then
				IsValidUsername = False
			Else
				IsValidUsername = True
				For Each Chr As Char In Text.ToCharArray
					If Not (Char.IsLetterOrDigit(Chr) Or Chr = "_"c Or Chr = "-"c Or Chr = " "c) Then
						Return False
					End If
				Next
			End If
		End Function

    Public Function IsValidSmsPhoneNumber(ByVal Text As String, Optional ByVal MinLength As Integer = 12, Optional ByVal MaxLength As Integer = 15) As Boolean
      If Text.Length >= MinLength And Text.Length <= MaxLength Then
        If Text.Chars(0) = "+" Then
          For N As Integer = 1 To Text.Length - 1
            If Not IsNumber(Text.Chars(N)) Then
              Return False
            End If
            Return True
          Next
        End If
      End If
      Return False
    End Function

    Public Function IsValidFaxNumber(ByVal Text As String, Optional ByVal MinLength As Integer = 10, Optional ByVal MaxLength As Integer = 16) As Boolean
      Return IsValidSmsPhoneNumber(Text, MinLength, MaxLength)
    End Function

    Public Function IsAlphaNumeric(ByRef Text As String) As Boolean
      If Text Is Nothing Then
        Return False
      ElseIf Text = "" Then
        Return False
      Else
        For Each C As Char In Text.ToCharArray
          If Not System.Char.IsLetterOrDigit(C) Then
            Return False
          End If
        Next
        Return True
      End If
    End Function

		Public Sub RegisterAnonimus()
			If CurrentUser.Role(CurrentSubSiteName) = Authentication.User.RoleType.Visitors Then
        HttpContext.Current.Session("ReturnUrl") = HttpContext.Current.Request.Url.ToString
        HttpContext.Current.Response.Redirect(HrefLogin())
			End If
			If AuthenticationIsRequired() Then
				HttpContext.Current.Response.Redirect(PayLinkForAuthentication(HttpContext.Current.Request.Url.ToString))
			End If
		End Sub

		Public Function HrefLogin(Optional NewUser As Boolean = True) As String
			If NewUser Then
				'Register new user
        Return Href(CurrentSubSiteName, False, "log.aspx", "a", CStr(LogActionType.NewUser))
			Else
				'Login existing user
				Return Href(CurrentSubSiteName, False, "log.aspx")
			End If
		End Function

		Public Property CurrentUser(Optional ByVal Session As System.Web.SessionState.HttpSessionState = Nothing) As Authentication.User
			Get
				If Session Is Nothing Then
					Try	'NOT remove this command! "Try" is necessary!
						Session = HttpContext.Current.Session
					Catch ex As Exception

					End Try
				End If
				'Session is notting when cookie is disabled!
				If Session IsNot Nothing Then
					If Session("user") IsNot Nothing Then
            Return CType(Session("user"), User)
					Else
						Dim User As New Authentication.User
						User.SetUser()
						Session("user") = User
						User.LastSessionID = Session.SessionID
						Return User
					End If
				Else
					Return New Authentication.User
				End If
			End Get
			Set(ByVal Value As Authentication.User)
				If Session Is Nothing Then
					Session = HttpContext.Current.Session
				End If
				'Session is notting when cookie is disabled!
				If Not Session Is Nothing Then
					Session("user") = Value
					Value.LastSessionID = Session.SessionID
				End If
			End Set
		End Property

		Public OnLineUsers As New System.Collections.Generic.Dictionary(Of String, OnlineUser)

		Class OnlineUser
			Public IsCrawler As Boolean
			Public User As User
			Public Domain As String
		End Class

		Sub AddOnLineUser(ByVal Session As Web.SessionState.HttpSessionState)
			CurrentUser(Session).SetOnline(Session)
			SyncLock OnLineUsers
				If OnLineUsers.ContainsKey(Session.SessionID) Then
					OnLineUsers.Remove(Session.SessionID)
				End If
				Dim OnlineUser As New OnlineUser
				OnlineUser.User = CurrentUser(Session)
				OnlineUser.IsCrawler = IsCrawler()
				OnlineUser.Domain = CurrentDomain()
				OnLineUsers.Add(Session.SessionID, OnlineUser)
			End SyncLock
		End Sub

		Sub RemoveOnLineUser(ByVal Session As Web.SessionState.HttpSessionState)
			CurrentUser(Session).SetOffline(Session)
			SyncLock OnLineUsers
				If OnLineUsers.ContainsKey(Session.SessionID) Then
					OnLineUsers.Remove(Session.SessionID)
				End If
			End SyncLock
		End Sub

		Public Function LogIn(ByVal UserName As String, Optional ByVal Password As String = Nothing) As Boolean
			Dim User As User = User.Load(UserName)
      Return LogIn(User, Password)
    End Function

		Public Function LogIn(ByVal User As User, Optional ByVal Password As String = Nothing) As Boolean
			'Try to take user from online user list
			If User IsNot Nothing Then
				User.SetUser()
				User.SubSite = CurrentSubSiteName()
				User.DetectTimeOffsetDisabled()
				'Session is notting when cookie is disabled!
				If Not HttpContext.Current.Session Is Nothing Then
					User.LastSessionID = HttpContext.Current.Session.SessionID
				End If

				If Password Is Nothing OrElse StrComp(Password, User.Password, CompareMethod.Text) = 0 Then
					If User.GeneralRole = Authentication.User.RoleType.Excluded Then
						Cookie("excludeduser") = "True"
					Else
						Dim LastUserName As String = Cookie("LastUserName")
						If LastUserName <> "" Then
              If StrComp(LastUserName, User.Username, CompareMethod.Text) <> 0 Then
                User.Generate = LastUserName
                'User.Save() 'This command is not necessary: User is saved at logout
              End If
						End If
						If Not AuthenticationIsRequired() Then
							If DateDiff(DateInterval.Day, User.FirstAccess.ToUniversalTime, Now.ToUniversalTime) >= 7 Then
								If User.GeneralRole = User.RoleType.User Then
									If User.Generate = "" Then
										User.GeneralRole = User.RoleType.Senior
									End If
								End If
							End If
						End If

						CookieUserName = User.Username
						Cookie("LastUserName") = User.Username
					End If
					RemoveOnLineUser(HttpContext.Current.Session)
					CurrentUser = User
					AddOnLineUser(HttpContext.Current.Session)
					Return True
				End If
			End If
      Return False
    End Function

		Public Sub TryAutoLogin()
			'Session is notting when cookie is disabled!
			If Not HttpContext.Current.Request.Path.EndsWith("/log.aspx") Then
				Dim UserName As String = CookieUserName
				If UserName <> "" Then
					Dim User As User = User.Load(UserName)
					If User IsNot Nothing Then
						If User.Generate <> "" Then
							'Block clone of banned user
							Dim AliasUser As User = User.Load(User.Generate)
							If AliasUser IsNot Nothing AndAlso AliasUser.GeneralRole = Authentication.User.RoleType.Excluded Then
								LockUser(User)
							End If
						End If
						If User.AutoLogIn Then
							Authentication.LogIn(User)
						End If
					End If
				End If
			End If
		End Sub

		Sub LogOutAllUsers()
			For Each OnlineUser As OnlineUser In OnLineUsers.Values
				OnlineUser.User.LogOut()
			Next
		End Sub

		Sub SendEmailToUser(ByVal User As User, ByVal Setting As SubSite, Optional LostPassword As Boolean = False)

			If User.Email <> "" Then
				Dim SubSite = Setting.Name
				Dim Language As LanguageManager.Language = User.Language

				Dim URL As New WebControls.HyperLink
				URL.Text = Href(SubSite, True, "default.aspx")
				URL.NavigateUrl = URL.Text

				Dim Subject As String
				Dim Body As String = Phrase(Language, 34) & "!<br />"
				If LostPassword = False Then
					Body &= Phrase(Language, 3059) & "<br />"
					Subject = Phrase(Language, 34) & "!"
				Else
					Subject = Phrase(Language, 321) & ": " & URL.Text
				End If
				Body &= ControlToText(URL) & "<br />" & _
				ControlToText(Components.InfoUser(User)) & "<br />"

				If LostPassword = False Then
					If AuthenticationIsRequired() Then
						Dim Authentication As New HyperLink
						Authentication.Text = Phrase(Language, 13) & ": " & Phrase(Language, 5)
						Authentication.NavigateUrl = PayLinkForAuthentication()
						Body &= ControlToText(Authentication) & "<br />"
					End If

          If Setting.Forums IsNot Nothing AndAlso CBool(Setting.Forums.Count) AndAlso Setting.ForumOfPresentation.InviteNewUsersToWriteTheirPresentation Then
            Dim ForumId As Integer, SubcategoryId As Integer
            If Setting.Forums.Contains(Setting.ForumOfPresentation.ForumId) Then
              ForumId = Setting.ForumOfPresentation.ForumId
            Else
              ForumId = Setting.Forums(0)
            End If
            Dim Forum As Forum = CType(Forum.Load(ForumId.ToString), ForumManager.Forum)
            If Forum IsNot Nothing Then
              Dim Subcategory As ForumManager.ForumStructure.Category.Subcategory = Forum.SubCategory(Setting.ForumOfPresentation.SubcategoryId)
              If Subcategory IsNot Nothing Then
                SubcategoryId = Setting.ForumOfPresentation.SubcategoryId
              End If
            End If

            Dim Presentation As New WebControls.HyperLink
            Presentation.Text = Phrase(Language, 3060)
            Presentation.NavigateUrl = Common.Href(CurrentDomainConfiguration, SubSite, True, "forum.aspx", "a", CStr(ForumManager.ActionType.NewTopic), "f", ForumId.ToString, "c", SubcategoryId.ToString, "TopicTitle", Phrase(Language, 3061) & " " & User.Username)
            Body &= ControlToText(Presentation) & "<br />"
          End If
				End If

				SendEmail(Subject, Body, User.Email, , , False)
			End If
		End Sub
		Function AuthenticationIsRequired() As Boolean
			If CurrentUser.GeneralRole = User.RoleType.User Then
				If Setup.Security.AuthenticateNewUsersThroughMicropayment.DoNotAuthenticateUsersSubscribedToTheSitesWithTheForumId IsNot Nothing Then
					For Each IdForum As Integer In Setup.Security.AuthenticateNewUsersThroughMicropayment.DoNotAuthenticateUsersSubscribedToTheSitesWithTheForumId
						Dim Setting As SubSite = CurrentSetting()
						If Setting.Forums IsNot Nothing Then
							For Each Forum As Integer In CurrentSetting.Forums
								If IdForum = Forum Then
									Return False
								End If
							Next
						End If
					Next
				End If
				If Setup.Security.AuthenticateNewUsersThroughMicropayment.Payment.Amount <> 0 AndAlso PayPalAccountIsSetting() Then
					Return True
				End If
			End If
      Return False
    End Function
		Function PayLinkForAuthentication(Optional ByVal Redirect As String = Nothing) As String
			Dim HostName As String = PathCurrentUrl()
			Dim Amount As Double = Setup.Security.AuthenticateNewUsersThroughMicropayment.Payment.Amount
			Dim Currency As String = Setup.Security.AuthenticateNewUsersThroughMicropayment.Payment.Currency
			If Currency.Length <> 2 Then
				Currency = "EUR"
			Else
				Currency = UCase(Currency)
			End If
			Return PayPalRequirePaymentUrl(Currency, Amount, "(" & HostName & ") " & Phrase(CurrentSetting.Language, 13), Phrase(CurrentSetting.Language, 5) & " " & CurrentUser.Username, CurrentSetting.Language, Redirect, "newuserauthentication", CurrentUser.Username)
		End Function


		Public Function AllUsers() As User()
			Dim Root As String = Extension.MapPath(UsersSubDirectory)
			Dim dir As New System.IO.DirectoryInfo(Root)
			Dim Files As System.IO.FileSystemInfo() = dir.GetFileSystemInfos("*.xml")
      Dim Users As User() = Nothing
			For Each File As System.IO.FileSystemInfo In Files
				Dim UserName As String = System.Web.HttpUtility.UrlDecode(Left(File.Name, Len(File.Name) - 4), System.Text.Encoding.UTF8)
				Dim User As User = User.Load(UserName)
				If User IsNot Nothing Then
					If Users Is Nothing Then
						ReDim Users(0)
					Else
						ReDim Preserve Users(Users.GetUpperBound(0) + 1)
					End If
					Users(Users.GetUpperBound(0)) = User
				End If
			Next
			Return Users
		End Function

	End Module
End Namespace


