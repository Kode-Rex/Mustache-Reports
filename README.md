# Simple-Report-Server-Example
An example of using the simple report server code base to generate reports in docx then convert to pdf format using node and LibreOffice

The word file happens using [docx-templater](https://docxtemplater.com/) written in node to genereate docx reports based on a template.
Because docx-templater is so awesome you can use [angular expression](https://docxtemplater.readthedocs.io/en/latest/angular_parse.html) to modify reporting data. 

Have a look at reportRender.js's ReportRender.prototype.configureAngularExpressions method - you can add your own customer angular expression here.

The pdf conversion happens using LibreOffice. 

_You will need to make sure both node and libreoffice 5 need to be installed on your computer to run this example._
 
**Please note there is a pre-build hook to copy the NodeApp contents into the solution directory Simple.Report.Server.Example\Reporting\NodeApp from Simple.Report.Server.NodeJS**

Please note this example makes use of TddBuddy's Clean Architecture and Synchronous Process Execution packages. 
 
+ Synchronous Process Runner - DotNetCore
   - Install-Package TddBuddy.Synchronous.Process.Runner.DotNetCore
   - Source @ https://github.com/T-rav/Synchronous-Process-Runner-DotNetCore
 
+ Clean Architecture Domain - DotNetCore
   - Install-Package TddBuddy.CleanArchitecture.Domain.DotNetCore
   - Source @ https://github.com/T-rav/CleanArchitecture-DotNetCore.git
