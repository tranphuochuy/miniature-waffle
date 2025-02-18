var car = {
    make: "volvo",
    speed: 160,
    engine: {
        size: 2.0,
        make: "BMW",
        fuel: "petrol",
        pistons:[ {maker: "BMW"}, 
                {maker: "BMW2"}
                ]
    },
    drive: function(){return "drive";}
};

console.log(car.make);