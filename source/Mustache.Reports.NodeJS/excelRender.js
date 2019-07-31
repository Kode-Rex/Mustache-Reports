var xlsxTemplate = require('xlsx-template');

function ExcelRender(){}

ExcelRender.prototype.renderAsBase64 = function(templateContent, reportData, sheetNumber){
    var template = new xlsxTemplate(templateContent);

    // Perform substitution
    template.substitute(sheetNumber, reportData);

    // Get data
    var result = template.generate({ type: "base64" });
    return result;
};

module.exports = ExcelRender;