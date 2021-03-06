import $ from 'jquery';
import Login from './Login';
import Game from './Game';
import './style.less';

$(document).ready(() => {
    const login = new Login();
    const game = new Game();
    
    login.on('auth', res => {
        login.buttonText = 'Получение информации о пользователе...'; 
        VK.api('users.get', {ids: res.uid, fields: 'domain'}, userinfo => {
            login.hide();
            game.init({...res, ...userinfo.response[0]});
        })
    })
});

