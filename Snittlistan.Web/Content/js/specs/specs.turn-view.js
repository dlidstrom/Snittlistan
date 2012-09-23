describe("Turn view", function () {
    var turns_view;

    beforeEach(function () {
        var turns = new App.Collections.Turns();
        turns.reset([
            {
                turn: 2,
                startDate: '22 sep',
                endDate: '23 sep',
                rosters: [
                    {
                        team: 'Fredrikshof A',
                        location: 'Birka',
                        opponent: 'Stockholm IFK',
                        date: 'lördag den 22 september 2012, 10:00',
                        declined: {
                            count: 2,
                            names: 'Kjell Jansson, Christer Liedholm'
                        }
                    },
                    {
                        team: 'Fredrikshof F',
                        location: 'Bowl-O-Rama',
                        opponent: 'AIK F',
                        date: 'lördag den 22 september 2012, 11:40',
                        declined: {
                            count: 1,
                            names: 'Daniel Lidström'
                        }
                    },
                    {
                        team: 'Fredrikshof B',
                        location: 'Bowl-O-Rama',
                        opponent: 'Hellas B',
                        date: 'söndag den 23 september 2012, 10:00',
                        declined: null
                    }
                ]
            }
        ]);
        this.turns_view = new App.Views.Turns({ model: turns });
    });

    it("lists all matches", function () {
        expect(this.turnsView.$(".row-fluid", ".span4").length).toEqual(3);
    });
});