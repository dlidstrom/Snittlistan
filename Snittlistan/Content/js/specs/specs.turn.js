describe("Turn", function () {
    "use strict";
    var turn;

    beforeEach(function () {
        turn = new App.Turn({
            season: '2011-2012',
            number: 1,
            date_start: new Date('September 9, 2012'),
            date_end: new Date('September 10, 2012')
        });
    });

    it("should have a season", function () {
        expect(turn.get("season")).toEqual('2011-2012');
    });

    it("should have a number", function () {
        expect(turn.get("number")).toEqual(1);
    });

    it("should have a start date", function () {
        expect(turn.get("date_start")).toEqual(new Date('September 9, 2012'));
    });

    it("should have an end date", function () {
        expect(turn.get("date_end")).toEqual(new Date('September 10, 2012'));
    });

    it("can change number", function () {
        turn.set({ number: 2 });
        expect(turn.get('number')).toEqual(2);
    });
});