console.log("Hello world");
console.log(200);


console.log(true);

console.log(null);

var num = 10;

console.log(num);
console.log("string", 10.11, true, false, null, num);

var car = {
    make: "volvo",
    speed: 160
}

var car1 = {
    name: "volvo2",
    speed: 160,
}


function runcar(x){
    console.log(x);
}
runcar(car);
runcar(car1);
var a = 10;
var b = 20;
function chia(a,b){
    var c = a / b;
    console.log(c);
    
}
chia(a,b);
var moto ={
    name: "BMW",
    speed: 200,
}
function runMoto(moto){
    console.log("chiecxeten: "+ moto.name + "  no chay toc do baonhieu  " + moto.speed);
}
runMoto(moto);

function maytinh(a,b){

    function cong(a,b){
        var c = a+b;
        console.log(c);
    }
    function tru(a,b){
        var e = a-b;
        console.log(e);
    }
    function chia(a,b){
        var f = a / b;
        console.log(f);
    }
    function nhan(a,b){
        var g = a * b;
        console.log(g);
    }

}
// new function 
var maytinhmoi = new maytinh(10,2);
maytinhmoi.cong();
