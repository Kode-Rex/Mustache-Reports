var xlsxTemplate = require('xlsx-template');

function ExcelRender(){}

ExcelRender.prototype.renderAsBase64 = function(templateContent, reportData){
    var template = new xlsxTemplate(templateContent);

    // Replacements take place on first sheet
    var sheetNumber = 1;

    // Perform substitution
    template.substitute(sheetNumber, reportData);

    // Get data
    var result = template.generate({ type: "base64" });
    return result;
};

module.exports = ExcelRender;