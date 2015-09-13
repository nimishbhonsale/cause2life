'use strict';

/**
 * User model
 */
var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var UserSchema = new Schema({
    username:{type:String},
    password:{type:String},
    profileType:{type:String},
    profileDetails:Schema.Types.Mixed,
    seller:Schema.Types.Mixed
}, { strict: false });

module.exports = mongoose.model('User', UserSchema,'user');
