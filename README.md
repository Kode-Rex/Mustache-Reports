# Simple-Report-Server-Example
An example of using simple report server to generate reports in docx and pdf format

This is a working example of how to generate PDF reports using node and LibreOffice
The word file happens using docx-templater written in node to genereate docx reports based on a template
The pdf conversion happens using LibreOffice, both node and libreoffice 5 need to be installed on your computer to run this example.
 
**Please note there is a pre-build hook to copy the NodeApp contents into the solution directory Simple.Report.Server.Example\Reporting\NodeApp from Simple.Report.Server.NodeJS**

Please also note this example makes use of TddBuddy's Clean Architecture and Synchronous Process Execution packages. 
 
+ Synchronous Process Runner - DotNetCore
   - Install-Package TddBuddy.Synchronous.Process.Runner.DotNetCore
   - Source @ https://github.com/T-rav/Synchronous-Process-Runner-DotNetCore
 
+ Clean Architecture Domain - DotNetCore
   - Install-Package TddBuddy.CleanArchitecture.Domain.DotNetCore
   - Source @ https://github.com/T-rav/CleanArchitecture-DotNetCore.git
