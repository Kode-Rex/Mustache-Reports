var xlsxTemplate = require('xlsx-template');

function ExcelRender(){}

ExcelRender.prototype.renderAsBase64 = function(templateContent, reportData, sheetNumbers){
    var template = new xlsxTemplate(templateContent);

    // Perform substitution
	var numbers = sheetNumbers.split(',');
	numbers.forEach(sheetNumber=>template.substitute(parseInt(sheetNumber), reportData));

    // Get data
    var result = template.generate({ type: "base64" });
    return result;
};

module.exports = ExcelRender;