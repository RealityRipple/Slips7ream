Public Class WebClientCore
  Inherits Net.WebClient
  ''' <summary>
  ''' Create a new instance of the <see cref="WebClientCore" /> Class.
  ''' </summary>
  ''' <param name="useEvents">If set to <c>True</c>, failures will trigger events, if <c>False</c>, failures will throw errors.</param>
  Public Sub New(useEvents As Boolean)
    MyBase.New()
    c_Events = useEvents
    c_CookieJar = New Net.CookieContainer
    c_SendCookieJar = False
    c_Timeout = 120
    c_RWTimeout = 300
    c_HTVer = Net.HttpVersion.Version11
    c_ErrorBypass = True
    c_KeepAlive = True
    c_ManualRedirect = True
    c_ResponseURI = Nothing
    System.Net.ServicePointManager.Expect100Continue = False
  End Sub
  ''' <summary>
  ''' Create a new instance of the <see cref="WebClientCore" /> Class.
  ''' </summary>
  Sub New()
    MyBase.New()
    c_Events = False
    c_CookieJar = New Net.CookieContainer
    c_SendCookieJar = False
    c_Timeout = 120
    c_RWTimeout = 300
    c_HTVer = Net.HttpVersion.Version11
    c_ErrorBypass = True
    c_KeepAlive = True
    c_ManualRedirect = True
    c_ResponseURI = Nothing
    System.Net.ServicePointManager.Expect100Continue = False
  End Sub
  ''' <summary>
  ''' A <see cref="WebClientCore" /> Upload or Download request failure message containing an <see cref="Exception" />.
  ''' </summary>
  Public Class ErrorEventArgs
    Inherits EventArgs
    Public [Error] As Exception
    ''' <summary>
    ''' Create a new instance of the <see cref="ErrorEventArgs" /> Class for use with the <see cref="Failure" /> event.
    ''' </summary>
    ''' <param name="Err">The Exception being passed through the <see cref="Failure" /> event.</param>
    Public Sub New(Err As Exception)
      [Error] = Err
    End Sub
  End Class
  ''' <summary>
  ''' This constant replaces any commas in the values of cookies received, and will be replaced by commas on any cookies being sent.
  ''' </summary>
  Public Const REPLACE_COMMA As String = "[COMMA]"
  ''' <summary>
  ''' This constant replaces any semicolons in the values of cookies received, and will be replaced by semicolons on any cookies being sent.
  ''' </summary>
  Public Const REPLACE_SEMIC As String = "[SEMICOLON]"
  Private c_CookieJar As Net.CookieContainer
  ''' <summary>
  ''' Acts as the container for a collection of <see cref="System.Net.CookieCollection" /> objects for use with an instance of the <see cref="WebClientCore" /> class.
  ''' </summary>
  ''' <value>A new CookieJar to use in place of the default empty Jar. This is useful for sharing and storing sessions and cookies across multiple <see cref="WebClientCore" /> instances.</value>
  ''' <returns>The current CookieJar for this <see cref="WebClientCore" />.</returns>
  Public Property CookieJar As Net.CookieContainer
    Get
      Return c_CookieJar
    End Get
    Set(value As Net.CookieContainer)
      c_CookieJar = value
    End Set
  End Property
  Private c_SendCookieJar As Boolean
  ''' <summary>
  ''' The way cookies are sent can be modified using this property.
  ''' </summary>
  ''' <value>Cookies will be sent by text if this value is <c>False</c>, and by both text and cookie jar if <c>True</c>.</value>
  ''' <returns>See the value entry for details on the return values.</returns>
  Public Property SendCookieJar As Boolean
    Get
      Return c_SendCookieJar
    End Get
    Set(value As Boolean)
      c_SendCookieJar = value
    End Set
  End Property
  Private c_ResponseURI As Uri
  ''' <summary>
  ''' The <see cref="Uri" /> of the last <see cref="GetWebResponse" /> event after any redirection.
  ''' </summary>
  ''' <returns>Initially, the value will be <c>Nothing</c> until a <see cref="GetWebResponse" /> event occurs. It may be equal to the <see cref="Uri" /> sent in a Download or Upload request, or it may be modified during the request as per page redirection standards*.</returns>
  ''' <remarks>*Page Redirection can be disabled using the <see cref="ManualRedirect" /> property. The standards followed if <see cref="ManualRedirect" /> is set to <c>False</c> are the HTTP 300 Status Code set of errors. Up to 50 redirects are allowed if <see cref="Net.HttpWebRequest.MaximumAutomaticRedirections" /> has not been set manually.</remarks>
  Public ReadOnly Property ResponseURI As Uri
    Get
      Return c_ResponseURI
    End Get
  End Property
  Private c_Timeout As Integer
  ''' <summary>
  ''' Gets or sets the time-out value in seconds for the <see cref="System.Net.HttpWebRequest.GetResponse" /> and <see cref="System.Net.HttpWebRequest.GetRequestStream" /> methods.
  ''' </summary>
  ''' <value></value>
  ''' <returns>The number of milliseconds to wait before the request times out. The default is 120 seconds.</returns>
  Public Property Timeout As Integer
    Get
      Return c_Timeout
    End Get
    Set(value As Integer)
      c_Timeout = value
    End Set
  End Property
  Private c_RWTimeout As Integer
  ''' <summary>
  ''' Gets or sets a time-out in seconds when writing to or reading from a stream.
  ''' </summary>
  ''' <value></value>
  ''' <returns>The number of seconds before the writing or reading times out. The default value is 300 seconds (5 minutes).</returns>
  Public Property ReadWriteTimeout As Integer
    Get
      Return c_RWTimeout
    End Get
    Set(value As Integer)
      c_RWTimeout = value
    End Set
  End Property
  Private c_HTVer As Version
  ''' <summary>
  ''' Gets or sets the version of HTTP to use for the request.
  ''' </summary>
  ''' <value></value>
  ''' <returns>The HTTP version to use for the request. The default is <see cref="System.Net.HttpVersion.Version11" />.</returns>
  Public Property HTTPVersion As Version
    Get
      Return c_HTVer
    End Get
    Set(value As Version)
      c_HTVer = value
    End Set
  End Property
  Private c_ErrorBypass As Boolean
  ''' <summary>
  ''' Gets or sets a <see cref="Boolean" /> value which determines how the <see cref="WebClientCore" /> reacts to the HTTP 400 Status Code set of errors.
  ''' </summary>
  ''' <value>If set to <c>True</c> and the error contains a <see cref="System.Net.WebResponse" /> value, that response is sent instead of an error.</value>
  ''' <returns>If <c>True</c>, errors may be bypassed and trigger Upload and Download completion events, if possible. If <c>False</c>, all error responses are treated as errors. The default value is <c>True</c>.</returns>
  Public Property ErrorBypass As Boolean
    Get
      Return c_ErrorBypass
    End Get
    Set(value As Boolean)
      c_ErrorBypass = value
    End Set
  End Property
  Private c_KeepAlive As Boolean
  ''' <summary>
  ''' Gets or sets a value that indicates whether to make a persistent connection to the Internet resource.
  ''' </summary>
  ''' <value>If set to <c>True</c>, the <see cref="WebClientCore" /> will send a Connection HTTP header with the value Keep-alive; if <c>False</c>, Close.</value>
  ''' <returns><c>True</c> if the request to the Internet resource should contain a Connection HTTP header with the value Keep-alive; otherwise, <c>False</c>. The default is <c>True</c>.</returns>
  Public Property KeepAlive As Boolean
    Get
      Return c_KeepAlive
    End Get
    Set(value As Boolean)
      c_KeepAlive = value
    End Set
  End Property
  Private c_ManualRedirect As Boolean
  ''' <summary>
  ''' Gets or sets a <see cref="Boolean" /> value which determines how the <see cref="WebClientCore" /> reacts to the HTTP 300 Status Code set of errors.
  ''' </summary>
  ''' <value></value>
  ''' <returns><c>False</c> if the request should automatically follow redirection responses from the Internet resource; otherwise, <c>True</c>. The default value is <c>True</c>.</returns>
  Public Property ManualRedirect As Boolean
    Get
      Return c_ManualRedirect
    End Get
    Set(value As Boolean)
      c_ManualRedirect = value
    End Set
  End Property
  Private c_Events As Boolean
  ''' <summary>
  ''' A <see cref="WebClientCore" /> Upload or Download request has failed.
  ''' </summary>
  ''' <param name="sender">The class which is triggering the event.</param>
  ''' <param name="e">The exception contained in an EventArg</param>
  Public Event Failure(sender As Object, e As ErrorEventArgs)
  ''' <summary>
  ''' The User Agent for Satellite Restriction Tracker
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  Public Shared ReadOnly Property UserAgent As String
    Get
      Return "Mozilla/5.0 (" & Environment.OSVersion.VersionString & "; CLR: " & GetCLRVersion & ") " & My.Application.Info.ProductName.Replace(" ", "") & "/" & My.Application.Info.Version.ToString
    End Get
  End Property
  Private m_Request As Net.WebRequest
  Private m_Result As Net.WebResponse
  Protected Overrides Function GetWebRequest(address As System.Uri) As System.Net.WebRequest
    Try
      If m_Request IsNot Nothing Then
        m_Request.Abort()
        m_Request = Nothing
      End If
      If m_Result IsNot Nothing Then
        m_Result.Close()
        m_Result = Nothing
      End If
      Dim request As Net.WebRequest = MyBase.GetWebRequest(address)
      If request.GetType Is GetType(Net.HttpWebRequest) Then
        CType(request, Net.HttpWebRequest).UserAgent = WebClientCore.UserAgent
        CType(request, Net.HttpWebRequest).ReadWriteTimeout = c_RWTimeout * 1000
        CType(request, Net.HttpWebRequest).Timeout = c_Timeout * 1000
        CType(request, Net.HttpWebRequest).CookieContainer = Nothing
        Dim sCookieHeader As String = c_CookieJar.GetCookieHeader(address)
        If Not String.IsNullOrEmpty(sCookieHeader) Then
          If sCookieHeader.Contains(REPLACE_COMMA) Or sCookieHeader.Contains(REPLACE_SEMIC) Then
            sCookieHeader = sCookieHeader.Replace(REPLACE_COMMA, ",")
            sCookieHeader = sCookieHeader.Replace(REPLACE_SEMIC, ";")
          End If
          CType(request, Net.HttpWebRequest).Headers.Add(Net.HttpRequestHeader.Cookie, sCookieHeader)
          If c_SendCookieJar Then
            Dim cNewJar As New Net.CookieContainer
            AppendCookies(cNewJar, "Cookie", CType(request, Net.HttpWebRequest).Headers, address.Host)
            CType(request, Net.HttpWebRequest).CookieContainer = cNewJar
          End If
        End If
        If address.LocalPath = "/wp-admin/admin-ajax.php" Then CType(request, Net.HttpWebRequest).Referer = "https://yourculture.uk/free-translation-service/"
        CType(request, Net.HttpWebRequest).AllowAutoRedirect = Not c_ManualRedirect
        CType(request, Net.HttpWebRequest).KeepAlive = c_KeepAlive
        CType(request, Net.HttpWebRequest).ProtocolVersion = HTTPVersion
        CType(request, Net.HttpWebRequest).CachePolicy = New Net.Cache.HttpRequestCachePolicy(Net.Cache.HttpRequestCacheLevel.BypassCache)
      End If
      m_Request = request
      Return request
    Catch ex As Net.WebException
      MyBase.CancelAsync()
      If c_Events Then
        RaiseEvent Failure(Me, New ErrorEventArgs(ex))
      Else
        Throw ex
      End If
      Return Nothing
    End Try
  End Function
  Protected Overrides Function GetWebResponse(request As System.Net.WebRequest) As System.Net.WebResponse
    Try
      Return HandleWebResponse(request, MyBase.GetWebResponse(request))
    Catch ex As Net.WebException
      If c_ErrorBypass Then Return HandleWebResponse(request, ex.Response)
      If ex.Message = "The request was aborted: The request was canceled." Then Return Nothing
      MyBase.CancelAsync()
      If c_Events Then
        RaiseEvent Failure(Me, New ErrorEventArgs(ex))
      Else
        Throw ex
      End If
      Return Nothing
    End Try
  End Function
  Protected Overrides Function GetWebResponse(request As System.Net.WebRequest, result As System.IAsyncResult) As System.Net.WebResponse
    Try
      Return HandleWebResponse(request, MyBase.GetWebResponse(request, result))
    Catch ex As Net.WebException
      Return HandleWebResponse(request, ex.Response)
      If ex.Message = "The request was aborted: The request was canceled." Then Return Nothing
      MyBase.CancelAsync()
      If c_Events Then
        RaiseEvent Failure(Me, New ErrorEventArgs(ex))
      Else
        Throw ex
      End If
      Return Nothing
    End Try
  End Function
  Private Sub WebClientCore_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
    If m_Request IsNot Nothing Then
      m_Request.Abort()
      m_Request = Nothing
    End If
    If m_Result IsNot Nothing Then
      m_Result.Close()
      m_Result = Nothing
    End If
  End Sub
#Region "Helper Functions"
  Private Function HandleWebResponse(request As Net.WebRequest, response As Net.WebResponse) As Net.WebResponse
    Try
      If response Is Nothing Then Return Nothing
      AppendCookies(c_CookieJar, "Set-Cookie", response.Headers, response.ResponseUri.Host)
      If response.GetType Is GetType(Net.HttpWebResponse) AndAlso Not String.IsNullOrEmpty(CType(response, Net.HttpWebResponse).CharacterSet) Then
        Dim charSet As String = CType(response, Net.HttpWebResponse).CharacterSet
        Try
          Me.Encoding = System.Text.Encoding.GetEncoding(charSet)
        Catch ex As Exception
          Me.Encoding = System.Text.Encoding.GetEncoding(28591)
        End Try
      ElseIf response.ContentType.ToLower.Contains("charset=") Then
        Dim charSet As String = response.ContentType.Substring(response.ContentType.ToLower.IndexOf("charset"))
        charSet = charSet.Substring(charSet.IndexOf("=") + 1)
        If charSet.Contains(";") Then charSet = charSet.Substring(0, charSet.IndexOf(";"))
        Try
          Me.Encoding = System.Text.Encoding.GetEncoding(charSet)
        Catch ex As Exception
          Me.Encoding = System.Text.Encoding.GetEncoding(28591)
        End Try
      End If
      m_Result = response
      c_ResponseURI = response.ResponseUri
      Return response
    Catch ex As Net.WebException
      Return HandleWebResponse(request, ex.Response)
    End Try
  End Function
  Private Function CleanCookie(sCookieData As String) As String
    Dim sReconstruction As String = Nothing
    Do While Not String.IsNullOrEmpty(sCookieData)
      Dim cName As String = Nothing
      Dim cVal As String = Nothing
      Dim cExtra As String = Nothing

      If sCookieData.Contains("=") Then
        cName = sCookieData.Substring(0, sCookieData.IndexOf("="))
        sCookieData = sCookieData.Substring(sCookieData.IndexOf("=") + 1).TrimStart
      End If

      If Not String.IsNullOrEmpty(sCookieData) Then
        If sCookieData.Contains(";") Then
          cVal = sCookieData.Substring(0, sCookieData.IndexOf(";"))
          sCookieData = sCookieData.Substring(sCookieData.IndexOf(";") + 1).TrimStart
        ElseIf sCookieData.Contains(",") Then
          cVal = sCookieData.Substring(0, sCookieData.IndexOf(","))
          sCookieData = sCookieData.Substring(sCookieData.IndexOf(",") + 1).TrimStart
        Else
          cVal = sCookieData
          sCookieData = Nothing
        End If
      End If

      Do While Not String.IsNullOrEmpty(sCookieData) AndAlso sCookieData.Contains(";")
        Dim sSegment As String = sCookieData.Substring(0, sCookieData.IndexOf(";") + 1)
        If (sSegment.Contains("=") And sSegment.Contains(",")) AndAlso (sSegment.IndexOf(",") < sSegment.IndexOf("=")) Then Exit Do
        cExtra &= sCookieData.Substring(0, sCookieData.IndexOf(";") + 1) & " "
        sCookieData = sCookieData.Substring(sCookieData.IndexOf(";") + 1).TrimStart
      Loop
      If Not String.IsNullOrEmpty(sCookieData) Then
        If (sCookieData.Contains("=") And sCookieData.Contains(",")) AndAlso (sCookieData.IndexOf(",") < sCookieData.IndexOf("=")) Then
          cExtra &= sCookieData.Substring(0, sCookieData.IndexOf(","))
          sCookieData = sCookieData.Substring(sCookieData.IndexOf(",") + 1).TrimStart
        Else
          cExtra &= sCookieData
          sCookieData = Nothing
        End If
      End If

      If Not String.IsNullOrEmpty(sReconstruction) Then sReconstruction &= ","
      If String.IsNullOrEmpty(cExtra) Then
        sReconstruction &= cName & "=" & cVal.Replace(",", REPLACE_COMMA).Replace(";", REPLACE_SEMIC)
      Else
        sReconstruction &= cName & "=" & cVal.Replace(",", REPLACE_COMMA).Replace(";", REPLACE_SEMIC) & "; " & cExtra
      End If
    Loop
    Return sReconstruction
  End Function
  Private Function CookieStrToCookies(sCookieData As String, DefaultDomain As String) As Net.Cookie()
    Dim cookieList As New Collections.Generic.List(Of Net.Cookie)
    Do While Not String.IsNullOrEmpty(sCookieData)
      Dim cName As String = Nothing
      Dim cVal As String = Nothing
      Dim cExtra As String = Nothing
      If sCookieData.Contains("=") Then
        cName = sCookieData.Substring(0, sCookieData.IndexOf("="))
        sCookieData = sCookieData.Substring(sCookieData.IndexOf("=") + 1).TrimStart
      End If
      If Not String.IsNullOrEmpty(sCookieData) Then
        If sCookieData.Contains(";") Then
          cVal = sCookieData.Substring(0, sCookieData.IndexOf(";"))
          sCookieData = sCookieData.Substring(sCookieData.IndexOf(";") + 1).TrimStart
        ElseIf sCookieData.Contains(",") Then
          cVal = sCookieData.Substring(0, sCookieData.IndexOf(","))
          sCookieData = sCookieData.Substring(sCookieData.IndexOf(",") + 1).TrimStart
        Else
          cVal = sCookieData
          sCookieData = Nothing
        End If
      End If
      Do While Not String.IsNullOrEmpty(sCookieData) AndAlso sCookieData.Contains(";")
        Dim sSegment As String = sCookieData.Substring(0, sCookieData.IndexOf(";") + 1)
        If sSegment.Contains("=") And sSegment.Contains(",") Then
          Dim sSegID As String = sSegment.Substring(0, sSegment.IndexOf("="))
          If sSegID.ToLower = "expires" Then
            If sSegment.IndexOf(",") < sSegment.IndexOf("=") Then Exit Do
            If sSegment.Substring(sSegment.IndexOf(",") + 1).Contains(",") Then Exit Do
          Else
            Exit Do
          End If
        End If
        cExtra &= sCookieData.Substring(0, sCookieData.IndexOf(";") + 1) & " "
        sCookieData = sCookieData.Substring(sCookieData.IndexOf(";") + 1).TrimStart
      Loop
      If Not String.IsNullOrEmpty(sCookieData) Then
        If sCookieData.Contains("=") And sCookieData.Contains(",") Then
          Dim sSegID As String = sCookieData.Substring(0, sCookieData.IndexOf("="))
          If sSegID.ToLower = "expires" Then
            If sCookieData.IndexOf(",") < sCookieData.IndexOf("=") Then
              cExtra &= sCookieData.Substring(0, sCookieData.IndexOf(","))
              sCookieData = sCookieData.Substring(sCookieData.IndexOf(",") + 1).TrimStart
            ElseIf sCookieData.Substring(sCookieData.IndexOf(",") + 1).Contains(",") And sCookieData.IndexOf(",", sCookieData.IndexOf(",") + 1) > sCookieData.IndexOf("=") Then
              cExtra &= sCookieData.Substring(0, sCookieData.IndexOf(",", sCookieData.IndexOf(",") + 1))
              sCookieData = sCookieData.Substring(sCookieData.IndexOf(",", sCookieData.IndexOf(",") + 1) + 1).TrimStart
            Else
              cExtra &= sCookieData
              sCookieData = Nothing
            End If
          Else
            cExtra &= sCookieData.Substring(0, sCookieData.IndexOf(","))
            sCookieData = sCookieData.Substring(sCookieData.IndexOf(",") + 1).TrimStart
          End If
        Else
          cExtra &= sCookieData
          sCookieData = Nothing
        End If
      End If
      If String.IsNullOrEmpty(cExtra) Then
        cookieList.Add(New Net.Cookie(cName, cVal, "/", DefaultDomain))
      Else
        Dim sDomain As String = Nothing
        Dim sPath As String = Nothing
        Dim bHTTP As Boolean = False
        Dim bSecure As Boolean = False
        Dim sExpires As String = Nothing
        Dim sMaxAge As String = Nothing
        Dim iVersion As Integer = Integer.MinValue
        Dim Extras() As String = Split(cExtra, ";")
        For Each sExtra In Extras
          If sExtra.Contains("=") Then
            Dim sExtraKV() As String = Split(sExtra.Trim, "=", 2)
            Select Case sExtraKV(0).ToLower
              Case "path"
                sPath = sExtraKV(1)
              Case "domain"
                sDomain = sExtraKV(1)
              Case "expires"
                sExpires = sExtraKV(1)
              Case "max-age"
                sMaxAge = sExtraKV(1)
              Case "version"
                iVersion = CInt(sExtraKV(1))
              Case Else
                Debug.Print("Unknown Cookie Key: " & sExtraKV(0))
            End Select
          Else
            Select Case sExtra.Trim.ToLower
              Case "http"
                bHTTP = True
              Case "secure"
                bSecure = True
            End Select
          End If
        Next
        If String.IsNullOrEmpty(sDomain) Then sDomain = DefaultDomain
        If String.IsNullOrEmpty(sPath) Then sPath = "/"
        If iVersion = 1 Then If cVal.StartsWith("""") And cVal.EndsWith("""") Then cVal = cVal.Substring(1, cVal.Length - 2)
        Dim nC As New Net.Cookie(cName, cVal, sPath, sDomain)
        nC.HttpOnly = bHTTP
        nC.Secure = bSecure
        If Not String.IsNullOrEmpty(sExpires) Then If Not Date.TryParse(sExpires, nC.Expires) Then nC.Expires = Now.AddDays(1)
        If Not String.IsNullOrEmpty(sMaxAge) Then
          Dim dMaxAge As Double = Val(sMaxAge)
          If dMaxAge < 0 Then
            nC.Expires = Now.AddDays(1)
          ElseIf dMaxAge = 0 Then
            If nC.Expires.Year = 1 Then nC.Expires = Now.AddDays(1)
          Else
            nC.Expires = Now.AddSeconds(dMaxAge)
          End If
        End If
        'If Not iVersion = Integer.MinValue Then nC.Version = iVersion
        cookieList.Add(nC)
      End If
    Loop
    Return cookieList.ToArray
  End Function
  Private Sub AppendCookies(ByRef CookieJar As Net.CookieContainer, HeaderName As String, Headers As Net.WebHeaderCollection, DefaultDomain As String)
    For Each sHead In Headers.AllKeys
      If sHead = HeaderName Then
        Dim sCookieData As String = Headers(sHead)
        Dim sReconstruction As String = CleanCookie(sCookieData)
        Dim newCookies() As Net.Cookie = CookieStrToCookies(sReconstruction, DefaultDomain)
        For Each newCookie In newCookies
          CookieJar.Add(newCookie)
        Next
      End If
    Next
  End Sub

  Public Shared Function GetCLRVersion() As String
    Dim sCLR As String = Nothing
    Dim tMONO As Type = Nothing
    Try
      tMONO = Type.GetType("Mono.Runtime")
    Catch ex As Exception
    End Try
    If tMONO Is Nothing Then
      Dim CLRRelease As String = "0"
      Try
        If My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\NET Framework Setup\NDP\v4").OpenSubKey("Full") IsNot Nothing Then
          CLRRelease = CStr(My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full").GetValue("Release"))
        ElseIf My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\NET Framework Setup\NDP\v4").OpenSubKey("Client") IsNot Nothing Then
          CLRRelease = CStr(My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client").GetValue("Release"))
        End If
      Catch ex As Exception
        CLRRelease = "0"
      End Try
      If String.IsNullOrEmpty(CLRRelease) Then CLRRelease = "0"
      sCLR = Environment.Version.ToString
      If Not CLRRelease = "0" Then sCLR &= "_" & CLRRelease
    Else
      Dim myMethods() As Reflection.MethodInfo = tMONO.GetMethods(Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Static)
      For Each mInfo As Reflection.MethodInfo In myMethods
        If mInfo Is Nothing Then Continue For
        Dim mName As String = "Unknown"
        Try
          mName = mInfo.Name
        Catch ex As Exception
          mName = "Unknown"
        End Try
        If Not mName = "GetDisplayName" Then Continue For
        Dim mInvoke As String = Nothing
        Try
          mInvoke = CStr(mInfo.Invoke(Nothing, Nothing))
        Catch ex As Exception
          mInvoke = Nothing
        End Try
        If Not String.IsNullOrEmpty(mInvoke) Then
          If mInvoke.Contains(" ") Then
            sCLR = mInvoke.Substring(0, mInvoke.IndexOf(" "c))
            Exit For
          End If
        End If
      Next
      If Not String.IsNullOrEmpty(sCLR) Then
        sCLR = "4.0.30319.17020_" & sCLR
      Else
        If Environment.Version.Major = 4 And Environment.Version.Minor = 0 And Environment.Version.Build = 30319 Then
          Select Case Environment.Version.Revision
            Case 17020 : sCLR = "4.0.30319.17020"
            Case 42000 : sCLR = "4.0.30319.17020_4.4"
            Case Else : sCLR = "4.0.30319.17020_" & Environment.Version.Revision
          End Select
        Else
          sCLR = Environment.Version.ToString
        End If
      End If
    End If
    Return sCLR
  End Function
#End Region
End Class
