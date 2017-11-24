var fs = require('fs');
var reportRender = require('./reportRender.js');
var program = require('commander');

program
    .version('1.0.1')
    .option('-t, --template [value]', 'Path to docx template')
    .option('-d, --data [value]', 'Path to data file to')
    .parse(process.argv);

var templateContent = fs.readFileSync(program.template, "base64");
var dataFileContents = fs.readFileSync(program.data, "utf-8");

var data = JSON.parse(dataFileContents);
var render = new reportRender();
var reportAsBase64String = render.renderAsBase64(templateContent, data);

console.log(reportAsBase64String);