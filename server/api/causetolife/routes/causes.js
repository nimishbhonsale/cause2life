/* Routes for cause */

require('../model/Cause')
var express = require('express');
var router = express.Router();

var mongoose = require('mongoose'),
    Cause = mongoose.model('Cause');

/* POST causes listing. */
router.post('/', function(req, res, next) {
  var db = mongoose.connect("mongodb://localhost:27017/causetolife", function(err) {
    console.log(req.body);
    Cause.create(req.body, function (err, post) {
      if (err) return next(err);
      res.json(post);
    });
  });
});

/* GET causes listing. */
router.get('/', function(req, res, next) {
  var db = mongoose.connect("mongodb://localhost:27017/causetolife", function(err) {
    var causes = Cause.find(function (err, causes) {
      if(err)
        res.send('failure');
      else
       res.json(causes);
    });
  });
});

/* GET /causes/id */
router.get('/:id', function(req, res, next) {
  var db = mongoose.connect("mongodb://localhost:27017/causetolife", function(err) {
    Cause.findById(req.params.id, function (err, post) {
      if (err) return next(err);
      res.json(post);
    });
  });
});

/* PUT /causes/:id */
router.put('/:id', function(req, res, next) {
  var db = mongoose.connect("mongodb://localhost:27017/causetolife", function(err) {
    Cause.findByIdAndUpdate(req.params.id, req.body, function (err, post) {
      if (err) return next(err);
      res.json(post);
    });
  });
});

/* DELETE /causes/:id */
router.delete('/:id', function(req, res, next) {
  var db = mongoose.connect("mongodb://localhost:27017/causetolife", function(err) {
    Cause.findByIdAndRemove(req.params.id, req.body, function (err, post) {
      if (err) return next(err);
      res.json(post);
    });
  });
});
module.exports = router;
