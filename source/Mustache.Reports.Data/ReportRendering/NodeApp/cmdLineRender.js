var fs = require('fs');
var wordRender = require('./wordRender.js');
var program = require('commander');

program
    .version('2.0.0')
    .option('-t, --template [value]', 'Path to docx or xlsx template')
    .option('-d, --data [value]', 'Path to data file to')
    .option('-rt --reportType [word | excel]', 'The report type to run')
    .parse(process.argv);

var templateContent = fs.readFileSync(program.template, "base64");
var dataFileContents = fs.readFileSync(program.data, "utf-8");

var data = JSON.parse(dataFileContents);

var reportType = program.reportType.toLowerCase();

if (reportType == 'word') {
    var render = new wordRender();
    var reportAsBase64String = render.renderAsBase64(templateContent, data);
    console.log(reportAsBase64String);
}

if (reportType == 'excel') {
    console.log('Todo');
}