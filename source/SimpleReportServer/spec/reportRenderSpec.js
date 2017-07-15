describe("Report Render", function () {
	var reportRender = require("../reportRender.js");
	var fs = require('fs');
	
    describe("renderAsBase64", function () {
        it("should render report when model contains all templated fields", function () {
            //Arrange
            var expected = fs.readFileSync("spec/expected/TestTemplateWithNoImages-Expected.docx","base64");
			var reportTemplate = fs.readFileSync("spec/testTemplates/TestTemplateWithNoImages.docx","base64");
            var reportData = JSON.parse(fs.readFileSync("spec/testTemplates/TestTemplateWithNoImagesModel.json"));
            var reportGenerator = new reportRender();
            //Act
            var result = reportGenerator.renderAsBase64(reportTemplate, reportData);
            //Assert
			areDocumentsTheSame(expected, result);
        });
		
		it("should not render report model properly is missing templated fields", function () {
            //Arrange
            var expected = fs.readFileSync("spec/expected/TestTemplateWithNoImages-Expected.docx","base64");
			var reportTemplate = fs.readFileSync("spec/testTemplates/TestTemplateWithNoImages.docx","base64");
            var reportData = { "firstName2" : "Travis", "lastName2" : "Frisinger", "birthdate": "1981-04-29", "isUnicycleQualified": true, "hobbies" : [{"name":"Reading"},{"name":"Programming"},{"name":"Walking"}]};
            var reportGenerator = new reportRender();
            //Act
            var result = reportGenerator.renderAsBase64(reportTemplate, reportData);
            //Assert
			areDocumentsNotSame(expected, result);
        });
		
		it("should render report model when it contains images and all templated fields", function () {
            //Arrange
            var expected = fs.readFileSync("spec/expected/TestTemplateWithImages-Expected.docx","base64");
			var reportTemplate = fs.readFileSync("spec/testTemplates/TestTemplateWithImages.docx","base64");
            var reportData = JSON.parse(fs.readFileSync("spec/testTemplates/TestTemplateWithImagesModel.json"));
            var reportGenerator = new reportRender();
            //Act
            var result = reportGenerator.renderAsBase64(reportTemplate, reportData);
            //Assert                     
            fs.writeFileSync("foo2.docx", result, 'base64', function(err) {
                console.log(err);
            });
			areDocumentsTheSame(expected, result);
        });
		
		function areDocumentsTheSame(expected, result){
			var expectedLegnth = expected.length;
			var resultLength = result.length;
			// check length and then a portion at start and end since base64 giving different results each time
			expect(expectedLegnth).toEqual(resultLength);
			expect(expected.slice(0,600)).toEqual(result.slice(0,600));
			expect(expected.slice(expected.length-50)).toEqual(result.slice(result.length-50));
		}
		
		function areDocumentsNotSame(expected, result){
			var expectedLegnth = expected.length;
			var resultLength = result.length;
			// check length and then a portion at start and end since base64 giving different results each time
			expect(expectedLegnth).not.toEqual(resultLength);
			expect(expected.slice(expected.length-50)).not.toEqual(result.slice(result.length-50));
		}
    });
});