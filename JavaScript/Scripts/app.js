var photoBrowser = angular.module("photoBrowser", ['ngFileUpload', 'angularUtils.directives.dirPagination', 'ngSanitize'])
//ngFileUpload: https://github.com/danialfarid/ng-file-upload
//angularUtils, Pagination: https://github.com/michaelbromley/angularUtils/tree/master/src/directives/pagination

//Lägg till header för att kunna skicka in data till kontrollern
photoBrowser.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
}]);


//Controller för att lägga till foton.
photoBrowser.controller("addPhoto", function ($scope, $http, Upload) {
    //Kolla och förstå detta!
    $scope.newPhoto = function (imageUpload) {
        Upload.upload({
            url: "/Photos/Create",
            data: { Title: $scope.photo.title, "imageUpload": imageUpload, UserId: $scope.photo.userId, Url: $scope.imageUpload.name },
            headers: {
                'Content-Type': undefined,
                '__RequestVerificationToken': angular.element("input[name='__RequestVerificationToken']").val()
            }
        }).then(function (successMessage) {
            console.log(successMessage);
            $scope.successMessage = successMessage.data;
        }, function (errorMessage) {
            console.log("The request failed: " + errorMessage);
            $scope.successMessage = "Something went wrong, please try again!";
        });  
    };
});

//Controller för att hämta, ta bort bilder och visa samt ta bort overlay.
photoBrowser.controller("photoList", function ($scope, $http, $window) {

    //Pagination scopes, currentPage håller reda på vilken sida den är på, pageSize är hur många element som får plats på varje "sida".
    $scope.currentPage = 1;
    $scope.pageSize = 9;

    //Gets images from the PhotosController.

    var getPhotos = {
        method: "GET",
        url: "/Photos/index/",
        headers: {
            'Content-Type': 'application/json'
        }
    };
    //Funktionen som hämtar bilderna från kontrollern
    $http(getPhotos).then(function (result) {
        $scope.images = result.data;
        console.log(result.data);
    });

    /* Visa overlay och sätt in data i de olika scope från image 
    (kanske ändra om så att vi bara skickar in specifik data i funktionen)*/
    $scope.showOverlay = function (image) {
        $scope.title = image.Title;
        $scope.url = image.Url;
        $scope.userId = image.UserId;
        $scope.imageId = image.Id;
        document.getElementById("overlay").style.display = "block";
        document.getElementById("editForm").style.display = "none";
    };
    //Tar bort overlay diven.
    $scope.removeOverlay = function () {
        document.getElementById("overlay").style.display = "none";
    };
    //Ta bort en bild.
    $scope.deleteImage = function (id) {

        //Variabeln för att ta bort bilden med rätt information och header för att kunna skicka med token till controller.
        var deletePhoto = {
            method: "POST",
            url: "/Photos/Delete/" + id,
            headers: {
                'Content-Type': undefined,
                '__RequestVerificationToken': angular.element("input[name='__RequestVerificationToken']").val()
            }
        };
        //Delete funktionen, får tillbaka bilder igen igenom om funktionen lyckas. (Finns det något effektivare sätt att få tillbaka bilderna?)
        if($window.confirm("Do you want to delete this image?")){
            $http(deletePhoto).then
            (function (successMessage) {
                document.getElementById("overlay").style.display = "none";
                $http(getPhotos).then(function (result) {
                    $scope.images = result.data;
                });
            },
            function (errorMessage) {
                console.log(errorMessage);
            });
        }
    };


    $scope.editImage = function (id) {
        document.getElementById("editForm").style.display = "block";
        var editPhoto = {
            method: "POST",
            url: "/Photos/Edit/" + id,
            headers: {
                'Content-Type': undefined,
                '__RequestVerificationToken': angular.element("input[name='__RequestVerificationToken']").val()
            }
        }

    };
})
