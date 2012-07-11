describe("Turn view", function () {
    var server = sinon.fakeServer.create();
    var list = [{ }, { }, { }];

    beforeEach(function () {
        server.respondWith(
            'GET',
            /\/appointments/ ,
            [200,
                { "Content-Type": "application/json" }, JSON.stringify(list)]
        );
    });

    it("lists all matches", function () {
        expect($(".row-fluid", ".span4").length).toEqual(3);
    });
});