'use strict';
var myapp = angular.module('chat', ['ngAnimate']);
myapp.controller(
				'ChatController',
				function ($rootScope, $scope) {
				    $scope.messages = new Array();
				    $scope.disabled = true;
				    var chat = $.connection.chatHub;

				    chat.client.recieveData = function (message) {
				        $scope.messages.push({ data: message });
				        $scope.$apply();
				    };

				    $.connection.hub.start().done(function () {
				        $scope.disabled = false;
				        $scope.$apply();
				    });

				    $scope.send = function () {
				        chat.server.send($scope.message);
				    };

				    $scope.clear = function () {
				        $scope.messages = new Array();
				    }

				});
