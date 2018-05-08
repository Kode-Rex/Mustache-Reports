var fs = require('fs');
var wordRender = require('./wordRender.js');
var excelRender = require('./excelRender.js');
var program = require('commander');

program
    .version('2.1.0')
    .option('-t, --template [value]', 'Path to docx or xlsx template')
    .option('-d, --data [value]', 'Path to data file to')
    .option('-r --reportType [word | excel]', 'The report type to run')
    .option('-n --sheetNumber <n>', 'This option is for when generating excel reports specifying the sheet number to target', parseInt)
    .parse(process.argv);

var stdout = process.stdout;

var reportData = fs.readFileSync(program.data, "utf-8");
var data = JSON.parse(reportData);

var reportType = program.reportType.toLowerCase();

if (reportType === 'word') {
    var templateContent = fs.readFileSync(program.template, "base64");
    var render = new wordRender();
    var reportAsBase64String = render.renderAsBase64(templateContent, data);
    stdout.write(reportAsBase64String);
}

if (reportType === 'excel') {
    var templateContent = fs.readFileSync(program.template, "binary");
    var render = new excelRender();

    var reportAsBase64String = render.renderAsBase64(templateContent, data, program.sheetNumber);
    stdout.write(reportAsBase64String);
}