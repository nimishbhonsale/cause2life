'use strict';

/**
 * Cause model
 */
var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var CauseSchema = new Schema({
    name:{type:String},
    description: {type:String},
    status:  {type:String},
    address: {type:String},
    assets: [Schema.Types.Mixed],
    votes:{type:Number},
    start:{type:Date},
    end:{type:Date},
    owners:[Schema.Types.String],
    sponsors:[Schema.Types.Mixed],
    testimonials:[Schema.Types.Mixed],
    budget: {type:Number},
    creator:{type:String}
}, { strict: false });

module.exports = mongoose.model('Cause', CauseSchema,'cause');
