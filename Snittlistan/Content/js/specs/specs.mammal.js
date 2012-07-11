describe("Mammal", function () {
    "use strict";
    var my_mammal;

    beforeEach(function () {
        my_mammal = Mammal.mammal({ name: "Daniel Lidstrom", saying: "meow" });
    });

    it("should have a name", function () {
        expect(my_mammal.get_name()).toEqual("Daniel Lidstrom");
    });

    it("should be able to say something", function () {
        expect(my_mammal.says()).toEqual("meow");
    });
})