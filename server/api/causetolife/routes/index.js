var express = require('express');
var router = express.Router();

/* GET home page. */
router.get('/', function(req, res, next) {
  res.render('index', { title: 'Cause to Life ..', content: 'Get a cause to life.' });
});

module.exports = router;
