var myApp = angular.module('myApp',['ngAnimate']);

myApp.config(function($locationProvider) {
  $locationProvider.html5Mode(true).hashPrefix('!');
});

myApp.controller('groupController', ['$scope', '$http', '$location',
function ($scope, $http, $location){
  $scope.prog = 'fun main(){\n  println("Hello World!");\n}';
  $scope.result = '';
  $scope.hideButton = false;

  if ($location.search().code){
      console.log($location.search().code);
      $scope.prog = unescape($location.search().code);
      $location.search('code',null);
  }

  $scope.link1 = $location.absUrl()+'?code='+escape($scope.prog);
  $scope.noLink = 0;
  $scope.genLink = function(){
    $scope.link1 = $location.absUrl()+'?code='+escape($scope.prog);
    console.log($scope.prog);
    console.log(escape($scope.prog))
  }

  var success = function(data){
    $scope.result = data;
    $scope.hideButton = false;
  };

  $scope.interpret = function(){
    $scope.hideButton = true;
    $http.post('http://tryfunwap.sfcoding.com/interpret', $scope.prog).success(function(data){
      success(data);
    });
  };

  $scope.compile = function(){
    $scope.hideButton = true;
    $http.post('http://tryfunwap.sfcoding.com/compile', $scope.prog).success(function(data){
      success(data);
    });
  };
}]);
