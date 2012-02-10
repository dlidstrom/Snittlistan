function ViewModel() {

    var team = initialData.HomeTeam;
    if (typeof team === "undefined" || team === null)
        team = initialData.Team;

    // first player
    this.team_1_player1 = ko.observable(team.Player1.Game1.Player);
    this.team_1_player1_strikes1 = ko.observable(team.Player1.Game1.Strikes);
    this.team_1_player1_strikes2 = ko.observable(team.Player1.Game2.Strikes);
    this.team_1_player1_strikes3 = ko.observable(team.Player1.Game3.Strikes);
    this.team_1_player1_strikes4 = ko.observable(team.Player1.Game4.Strikes);
    // second player
    this.team_1_player2 = ko.observable(team.Player2.Game1.Player);
    this.team_1_player2_strikes1 = ko.observable(team.Player2.Game1.Strikes);
    this.team_1_player2_strikes2 = ko.observable(team.Player2.Game2.Strikes);
    this.team_1_player2_strikes3 = ko.observable(team.Player2.Game3.Strikes);
    this.team_1_player2_strikes4 = ko.observable(team.Player2.Game4.Strikes);
    // third player
    this.team_1_player3 = ko.observable(team.Player3.Game1.Player);
    this.team_1_player3_strikes1 = ko.observable(team.Player3.Game1.Strikes);
    this.team_1_player3_strikes2 = ko.observable(team.Player3.Game2.Strikes);
    this.team_1_player3_strikes3 = ko.observable(team.Player3.Game3.Strikes);
    this.team_1_player3_strikes4 = ko.observable(team.Player3.Game4.Strikes);
    // fourth player
    this.team_1_player4 = ko.observable(team.Player4.Game1.Player);
    this.team_1_player4_strikes1 = ko.observable(team.Player4.Game1.Strikes);
    this.team_1_player4_strikes2 = ko.observable(team.Player4.Game2.Strikes);
    this.team_1_player4_strikes3 = ko.observable(team.Player4.Game3.Strikes);
    this.team_1_player4_strikes4 = ko.observable(team.Player4.Game4.Strikes);

    if (typeof initialData.AwayTeam !== "undefined" && initialData.AwayTeam !== null) {
        team = initialData.AwayTeam;
        // first player
        this.team_2_player1 = ko.observable(team.Player1.Game1.Player);
        this.team_2_player1_strikes1 = ko.observable(team.Player1.Game1.Strikes);
        this.team_2_player1_strikes2 = ko.observable(team.Player1.Game2.Strikes);
        this.team_2_player1_strikes3 = ko.observable(team.Player1.Game3.Strikes);
        this.team_2_player1_strikes4 = ko.observable(team.Player1.Game4.Strikes);
        // second player
        this.team_2_player2 = ko.observable(team.Player2.Game1.Player);
        this.team_2_player2_strikes1 = ko.observable(team.Player2.Game1.Strikes);
        this.team_2_player2_strikes2 = ko.observable(team.Player2.Game2.Strikes);
        this.team_2_player2_strikes3 = ko.observable(team.Player2.Game3.Strikes);
        this.team_2_player2_strikes4 = ko.observable(team.Player2.Game4.Strikes);
        // third player
        this.team_2_player3 = ko.observable(team.Player3.Game1.Player);
        this.team_2_player3_strikes1 = ko.observable(team.Player3.Game1.Strikes);
        this.team_2_player3_strikes2 = ko.observable(team.Player3.Game2.Strikes);
        this.team_2_player3_strikes3 = ko.observable(team.Player3.Game3.Strikes);
        this.team_2_player3_strikes4 = ko.observable(team.Player3.Game4.Strikes);
        // fourth player
        this.team_2_player4 = ko.observable(team.Player4.Game1.Player);
        this.team_2_player4_strikes1 = ko.observable(team.Player4.Game1.Strikes);
        this.team_2_player4_strikes2 = ko.observable(team.Player4.Game2.Strikes);
        this.team_2_player4_strikes3 = ko.observable(team.Player4.Game3.Strikes);
        this.team_2_player4_strikes4 = ko.observable(team.Player4.Game4.Strikes);
    }
};

ko.applyBindings(new ViewModel());
