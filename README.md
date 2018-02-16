# Mustache-Reports-Example
An example of using the mustache reports server code base to generate reports in docx using node and LibreOffice to convert to PDF.

_You will need to make sure both node and libreoffice 5 are installed on your computer to run this example._

_This example is rough and ready, please log an issue if you require assistance._

_There are now nuget packages which contain the core report rendering logic, removing the need to manually manage node packages in your solution_. 

The word file generations happens using [docx-templater](https://docxtemplater.com/) and [xlsx-template](https://www.npmjs.com/package/xlsx-template). It is written in node and uses mustache templating to populate the report.
You will need to model your report data in JSON for the templating to work, and because docx-templater is so awesome you can use [angular expression](https://docxtemplater.readthedocs.io/en/latest/angular_parse.html) to modify reporting data at runtime. It is unknown if xlsx-template also supports angular expressions, if not I will add it soon.

Have a look at _reportRender.js_ ReportRender.prototype.configureAngularExpressions method - you can add your own customer angular expressions here.  
_Please note there are currently plans to abstract this allowing your to specify your own angular expressions file without needing to modify the core package. This is one of my main focuses_

I have also added a custom image handling code to allow for base64 encoded image data to be part of the JSON payload - have a look at _reportRender.js_ ReportRender.prototype.getOptions method for more detail.
_Please note this is now a paid for feature of docx-templater, good thing we got the last version of the open source image module_

This example makes use of TddBuddy's Clean Architecture and Synchronous Process Execution packages. 
 
+ Synchronous Process Runner - DotNetCore
   - Install-Package TddBuddy.Synchronous.Process.Runner.DotNetCore
   - Source @ https://github.com/T-rav/Synchronous-Process-Runner-DotNetCore
 
+ Clean Architecture Domain - DotNetCore
   - Install-Package TddBuddy.CleanArchitecture.Domain.DotNetCore
   - Source @ https://github.com/T-rav/CleanArchitecture-DotNetCore.git
