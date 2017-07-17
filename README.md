# Simple-Report-Server-Example
An example of using the simple report server code base to generate reports in docx using node and LibreOffice to convert to PDF.

_You will need to make sure both node and libreoffice 5 are installed on your computer to run this example._

This example is rough and ready, please log an issue if you require assistance. 

The word file generations happens using [docx-templater](https://docxtemplater.com/). It is written in node and uses mustache templating to populate the report.
You will need to model your report data in JSON for the templating to work, and because docx-templater is so awesome you can use [angular expression](https://docxtemplater.readthedocs.io/en/latest/angular_parse.html) to modify reporting data. 

Have a look at reportRender.js's ReportRender.prototype.configureAngularExpressions method - you can add your own customer angular expressions here.

I have also added a custom image handling code to allow for base64 encoded image data to be part of the JSON payload. Have a look at reportRender.js's ReportRender.prototype.getOptions method for more detail.

_Please note there is a pre-build hook to copy the NodeApp contents into the solution directory Simple.Report.Server.Example\Reporting\NodeApp from Simple.Report.Server.NodeJS_

This example makes use of TddBuddy's Clean Architecture and Synchronous Process Execution packages. 
 
+ Synchronous Process Runner - DotNetCore
   - Install-Package TddBuddy.Synchronous.Process.Runner.DotNetCore
   - Source @ https://github.com/T-rav/Synchronous-Process-Runner-DotNetCore
 
+ Clean Architecture Domain - DotNetCore
   - Install-Package TddBuddy.CleanArchitecture.Domain.DotNetCore
   - Source @ https://github.com/T-rav/CleanArchitecture-DotNetCore.git
