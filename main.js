var editor = ace.edit("editor");
    editor.setTheme("ace/theme/tomorrow");
    editor.getSession().setTabSize(2);

var myApp = angular.module('myApp',['ngAnimate']);

myApp.config(function($locationProvider) {
  $locationProvider.html5Mode(true).hashPrefix('!');
});

myApp.controller('groupController', ['$scope', '$http', '$location',
function ($scope, $http, $location){
  var doc = editor.getSession().getDocument();
  $scope.result = '';
  $scope.hideButton = false;

  if ($location.search().code){
    doc.setValue(decodeURIComponent($location.search().code));
    $location.search('code',null);
  }else
    doc.setValue('fun main(){\n  println("Hello World!");\n}');

  var genUrl = function(){
    return $location.absUrl()+'?code='+encodeURIComponent(doc.getValue());
  }
  $scope.link1 = genUrl();
  $scope.noLink = 0;

  doc.on("change", function(){
    $scope.link1 = genUrl();
    $scope.$digest();
  });

  var success = function(data){
    $scope.result = data;
    $scope.hideButton = false;
  };

  $scope.interpret = function(){
    $scope.hideButton = true;
    $http.post('http://tryfunwap.sfcoding.com/interpret', doc.getValue()).success(function(data){
      success(data);
    });
  };

  $scope.compile = function(){
    $scope.hideButton = true;
    $http.post('http://tryfunwap.sfcoding.com/compile', doc.getValue()).success(function(data){
      success(data);
    });
  };
}]);
