var myApp = angular.module('myApp',[]);

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

  $scope.link = '';
  $scope.noLink = true;
  $scope.genLink = function(){
    $scope.link = $location.absUrl()+'?code='+escape($scope.prog);
    $scope.noLink = false;
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
