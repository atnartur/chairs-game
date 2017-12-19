import $ from 'jquery';
import Login from './login';
import Game from './Game';
import './style.less';

$(document).ready(() => {
    const login = new Login();
    const game = new Game();
    
    login.on('auth', res => {
        login.hide();
        game.init(res.session);
    })
});

