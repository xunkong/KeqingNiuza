StandardSocketsHttpHandler:
===========================
StandardSocketsHttpHandler is a backport of SocketsHttpHandler to .NET Standard 2.0.

Features added to StandardSocketsHttpHandler:
=============================================
• .NET Standard 2.0 compatibility (.NET Framework 4.7.2 is supported)  
• ConnectCallback property - allows configuration of various socket options (TCP Keepalive, etc.)  

Features missing from StandardSocketsHttpHandler:
=============================================
• Windows Authentication (NLTM & Kerberos)  
• Operating System proxy settings detection  

Using StandardSocketsHttpHandler:
=================================
HttpClient client = new HttpClient(new StandardSocketsHttpHandler())  
 
Releases:
=========
Releases are available via [NuGet.org](https://www.nuget.org/packages/StandardSocketsHttpHandler)  

Contact:
========
If you have any question, feel free to contact me.  
Tal Aloni <tal.aloni.il@gmail.com>  
