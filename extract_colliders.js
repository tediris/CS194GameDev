_und = require("underscore")

function fn(obj, key) {
    if (_und.has(obj, key)) // or just (key in obj)
        return [obj];
    // elegant:
    return _und.flatten(_und.map(obj, function(v) {
        return typeof v == "object" ? fn(v, key) : [];
    }), true);

    // or efficient:
    var res = [];
    _und.forEach(obj, function(v) {
        if (typeof v == "object" && (v = fn(v, key)).length)
            res.push.apply(res, v);
    });
    return res;
}

if (process.argv.length < 3) {
	console.log('usage: node extract_colliders.js <FILENAME>')
} else {
	var data = require("./" + process.argv[2]);
	//console.log(TileMaps);
	objectList = fn(TileMaps, 'objects');
	console.log(JSON.stringify(objectList[0]['objects']));
}
