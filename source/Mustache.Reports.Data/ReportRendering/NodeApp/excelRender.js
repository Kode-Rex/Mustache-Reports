var xlsxTemplate = require('xlsx-template');

function ExcelRender(){}

ExcelRender.prototype.renderAsBase64 = function(templateContent, reportData, sheetNumber){
    var template = new xlsxTemplate(templateContent);

    var x = 1;
    // Perform substitution
    template.substitute(x, reportData);

    // Get data
    var result = template.generate({ type: "base64" });
    return result;
};

module.exports = ExcelRender;