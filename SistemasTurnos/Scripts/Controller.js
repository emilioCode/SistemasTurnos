var MyApp = angular.module("myApp", []);

MyApp.controller('LoginController', function ($scope, $http) {

    $scope.servicios = [];
    $scope.tickets = [];
    $scope.ticketsFinishes = [];
    $scope.allTicketsAtende = [];
    $scope.roles = [];
    $scope.users = [];

    $scope.userName = '';
    $scope.password = '';

    //usuarios del formulario
    $scope.nameForm = '';
    $scope.documentForm = '';
    $scope.userForm = '';
    $scope.pwdForm = '';
    $scope.pwd2Form = '';
    $scope.rolForm = '';

    $scope.clearForm = function () {
        $scope.nameForm = '';
        $scope.documentForm = '';
        $scope.userForm = '';
        $scope.pwdForm = '';
        $scope.pwd2Form = '';
        $scope.rolForm = '';
    }
    //

    $scope.cleanMsg = function () {
        $scope.error_usuario = false;
        $scope.error_login = false;
    }
    $scope.cleanMsg();

 ;
    $scope.clear = function () {
        $scope.userName = '';
        $scope.password = '';
        $scope.userAccount
    }
    $scope.clear();

    $scope.logearse = function (user, pwd) {
        //console.log(user + " " + pwd);
        if (user == "" || user == null) {
            //$scope.error_usuario = true;
            swal('AVISO', 'Introduzca el usuario', 'warning');
            $scope.cleanMsg();
        } else {
            var data = {
                user: user,
                pwd:pwd
            }
                $http({
                    method: 'POST',
                    url: '/Home/Logearse',
                    data:data
                }).then(function successCallback(res) {
                    console.log(res.data);
                    //console.log(res.data);
                    if (res.data == null || res.data == '') {
                        //$scope.error_login = true;
                        $scope.clear();
                        swal('ACCESO DENEGADO', 'Intentelo nuevamente', 'warning');
                        
                    } else {
                        $scope.userAccount = res.data;
                    }
                    $scope.clear();
                }, function errorCallback(error) {
                    
                    console.error(error);
                });
        }
    };

    $scope.getTickets = function () {
        $scope.tickets = [];
        $http({
            method: 'POST',
            url: '/Home/getTickets'
        }).then(function successCallback(res) {
            //console.table(res.data);
            $scope.tickets = res.data;
            //swal('', 'OK', 'success')
            // this callback will be called asynchronously
            // when the response is available
        }, function errorCallback(error) {
            console.error(error);
        });
    }
    $scope.getTickets();

    $scope.getAllTicketsAtende = function () {
        $http({
            method: 'POST',
            url: '/Home/getAllTicketsAtende'
        }).then(function successCallback(res) {
           
            $scope.allTicketsAtende = res.data;
        }, function errorCallback(error) {
            console.error(error);
        });
    }
    $scope.getAllTicketsAtende();


    $scope.getServices = function () {
        $http({
            method: 'POST',
            url: '/Home/getServices'
        }).then(function successCallback(res) {
            console.table(res.data);
            $scope.servicios = res.data;
        }, function errorCallback(error) {
            console.error(error);
        });
    };
    $scope.getServices();

    $scope.atender = function (idticket,turno, iduser, codServicio,tipoServicio) {
        
        //console.log(tipoServicio + ' | ' + codServicio + '-' + turno)
        var data = {
            ticket: idticket,
            user:iduser
        }
        $http({
            method: 'POST',
            url: '/Home/atender',
            data:data
        }).then(function successCallback(res) {
            var r = res.data;
            swal(tipoServicio, 'TURNO:' + turno, 'success');
            $scope.getTickets();
            $scope.getTicketsFinishes();
        }, function errorCallback(error) {
            swal('', 'Ha ocurrido un error', 'error');
            console.error(error);
        });
    }

    $scope.getTicketsFinishes = function () {
        $http({
            method: 'POST',
            url: '/Home/getTicketsFinishes'
        }).then(function successCallback(res) {
            console.table(res.data);
            $scope.ticketsFinishes = res.data;
        }, function errorCallback(error) {
            console.error(error);
        });
    }
    $scope.getTicketsFinishes();



    $scope.deshabilitar = function (ticket) {
        $http({
            method: 'POST',
            url: '/Home/deshabilitar?id=' + ticket
        }).then(function successCallback(res) {
            $scope.getTickets();
            $scope.getTicketsFinishes();
        }, function errorCallback(error) {
            console.error(error);
        });
    }

    $scope.despachar = function (ticket) {
        $http({
            method: 'POST',
            url: '/Home/despachar?id=' + ticket
        }).then(function successCallback(res) {
    
            $scope.getTicketsFinishes();
            $scope.getAllTicketsAtende();
        }, function errorCallback(error) {
            console.error(error);
        });
    }
    $scope.TicketsforUser = [];
    $scope.getTicketsforUser = function () {
        $scope.TicketsforUser = [];
        $http({
            method: 'POST',
            url: '/Home/getTicketsforUser'
        }).then(function successCallback(res) {
            console.warn('here!');
            console.table(res.data)
            $scope.TicketsforUser = res.data;
        }, function errorCallback(error) {
            console.error(error);
        });
    }



    $scope.getRoles = function () {
        $http({
            method: 'POST',
            url: '/Home/getRoles'
        }).then(function successCallback(res) {
            console.log(res.data);
            $scope.roles = res.data;
        }, function errorCallback(error) {
            console.error(error);
        });
    };
    $scope.getRoles();

    $scope.createUser = function (nombre, ced, user, pwd, pwd2, rol) {
        if (pwd == pwd2) {
        var data = {
            NombreCompleto: nombre,
            Cedula: ced,
            UserName: user,
            Password: pwd,
            Rol: rol
        }
        $http({
            method: 'POST',
            url: '/Home/createUser',
            data:data
        }).then(function successCallback(res) {
            swal('Hecho', 'Usuario creado correctamente', 'success');
            
        }, function errorCallback(error) {
            console.error(error);
        });
        }else{
            swal('AVISO', 'Contraseña invalida', 'warning');
        }

        $scope.nameForm = '';
        $scope.documentForm = '';
        $scope.userForm = '';
        $scope.pwdForm = '';
        $scope.pwd2Form = '';
        $scope.rolForm = '';
       
    };

    $scope.getUser = function () {
       
            $http({
                method: 'POST',
                url: '/Home/getUserCreated'
            }).then(function successCallback(res) {
                $scope.users = res.data;
            }, function errorCallback(error) {
                console.error(error);
            });
       

    };

    $scope.ticketsInProcess = [];
    $scope.getTicketsInProcess = function () {
        $http({
            method: 'POST',
            url: '/Home/getTicketsInProcess'
        }).then(function successCallback(res) {
            //console.table(res.data);
            $scope.ticketsInProcess = res.data;
        }, function errorCallback(error) {
            console.error(error);
        });
    }

    //$scope.msg_pwd = false;
    //function WatchPwdFinal(){
    //    $scope.$watch('',function(newValue, oldValue){
    //        if (newValue != oldValue) {
    //            $scope.msg_pwd = true;
    //        } else {
    //            $scope.msg_pwd = false;
    //        }
          
    //    }, true);
    //};
    //WatchPwdFinal();

});


MyApp.controller('HomeController', function ($scope,$http) {

    $scope.servicios = [];
    $scope.clear = function () {
        $scope.tipoServicio = '';
    }
    $scope.clear();

    $scope.getServices = function () {
        $http({
            method: 'POST',
            url: '/Home/getServices'
        }).then(function successCallback(res) {
            console.table(res.data);
            $scope.servicios = res.data;
            //swal('', 'OK', 'success')
            // this callback will be called asynchronously
            // when the response is available
        }, function errorCallback(error) {
            console.error(error);
        });
    };
    $scope.getServices();

    $scope.setTurno = function (id) {
        console.error('paso');
        if (id == null || id == '') {
            swal('','Elija un servicio a realizar','error')
        } else {
            $http({
                method: 'POST',
                url: '/Home/setTurno?id=' + id
            }).then(function successCallback(res) {
                $scope.clear();

                if (res.data == 'Sistema de turnos cerrado, vuelva luego') {
                    swal('AVISO', res.data, 'warning')
                } else {
                    swal(res.data.split(' ')[0], "tiempo estimado: " + res.data.split(' ')[1] + " mins", 'success')
                }

                

            }, function errorCallback(error) {
                console.error(error);
            });
        }
       
    }
});


MyApp.controller('turnosController', function ($scope,$http) {

    $scope.title = 'SISTEMA DE TURNOS';
    $scope.title2 = 'PANTALLA DE ESPERA';
    $scope.TicketsWaiting = [];
    $scope.servicios = [];

    $scope.getTicketsWaiting = function () {
        $http({
            method: 'POST',
            url: '/Home/getTicketsWaiting'
        }).then(function successCallback(res) {
            //console.table(res.data);
            $scope.TicketsWaiting = res.data;
        }, function errorCallback(error) {
            console.error(error);
        });
    };
    $scope.getTicketsWaiting();

    $scope.getServices = function () {
        $http({
            method: 'POST',
            url: '/Home/getServices'
        }).then(function successCallback(res) {
            //console.table(res.data);
            $scope.servicios = res.data;
        }, function errorCallback(error) {
            console.error(error);
        });
    };
    $scope.getServices();


    function allTime() {
        setInterval(function () {
            console.info('loading...');
            $scope.getTicketsWaiting();
        }, 5000)
    }
    allTime();

});