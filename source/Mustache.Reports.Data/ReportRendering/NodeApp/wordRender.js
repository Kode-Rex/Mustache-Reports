var Docxtemplater = require('docxtemplater');
var ImageModule = require('docxtemplater-image-module');
var Moment = require('moment');
var JSZip = require('jszip');
var expressions = require('angular-expressions');

function WordRender(){}

WordRender.prototype.getOptions = function(){
	var opts = {}
	opts.centered = false;

	function generateTransparent1pxImage(){
		return new Buffer("R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7","base64").toString("binary");
	}

	opts.getImage = function (tagValue, tagName) {
		if(tagValue.data){
			 return new Buffer(tagValue.data,'base64').toString('binary');
		}

		return generateTransparent1pxImage();
	}

	opts.getSize = function (img, tagValue, tagName) {
		var height = 1;
		var width = 1;
		if(tagValue.width)
		{
			width = tagValue.width;
		}
		if(tagValue.height) {
		    height = tagValue.height;
		}
	    return [width, height];
	}

	return opts;
};

// todo : I need to find a nice generic way for people to extend this ;)
WordRender.prototype.configureAngularExpressions = function(){
	
	expressions.filters.upper = function(input) {
		if(!input) return input;
		return input.toUpperCase();
	}

	expressions.filters.formatBoolean = function(input){
		if(input === undefined) return input;
		return input === true ? "Yes" : "No";
	}
	
	expressions.filters.formatDate = function(input, format){
		if(!input) return input;
		return Moment(input).format(format).toString();
    }

	expressions.filters.today = function(input, format){
		return Moment().format(format).toString();
	}

	expressions.filters.doesStringHaveData = function(input){
		if(!input) return false;
		return input.length > 0;
	}

	expressions.filters.formatArrayAsList = function(input, delimiter){
		var result = "";
		if(!input || Object.prototype.toString.call( input ) !== '[object Array]') return result;
		input.forEach(function(item){
			result += item + delimiter;
		});
		return result.substring(0,result.length-1);
	}
};

WordRender.prototype.renderAsBase64 = function(reportTemplateBase64, reportData){
	var self = this;
	var opts = self.getOptions();
	var data = reportData;
 
	self.configureAngularExpressions();
	var angularParser = function(tag) {
		return {
			get: tag === '.' ? function(s){ return s;} : expressions.compile(tag)
		};
	}

	var imageModule = new ImageModule(opts);
	var zipOfReportTemplate = new JSZip(reportTemplateBase64, {base64: true});
	var doc = new Docxtemplater().setOptions({ parser: angularParser }).loadZip(zipOfReportTemplate);
	doc.attachModule(imageModule);
	doc.setData(data);
	doc.render();

	var result = doc.getZip().generate({ type: "base64" });
	return result;
};

module.exports = WordRender;