var assessment = artifacts.require("Assessment");

 module.exports = function(deployer) {
   // deployment steps
   deployer.deploy(assessment);
 };