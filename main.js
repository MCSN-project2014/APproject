function groupController($scope, $http){
  $scope.prog = 'fun main(){\n println("Hello World!");\n}';
  $scope.result = '';
  $scope.hideButton = false;

  var success = function(data){
    $scope.result = data;
    $scope.hideButton = false;
  };

  $scope.interpret = function(){
    console.log("send function");
    $scope.hideButton = true;
    $http.post('http://tryfunwap.sfcoding.com/interpret', $scope.prog).success(function(data){
      success(data);
    });
  };

  $scope.compile = function(){
    console.log("send function");
    $scope.hideButton = true;
    $http.post('http://tryfunwap.sfcoding.com/compile', $scope.prog).success(function(data){
      success(data);
    });
  };
}
