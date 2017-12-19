import MK from 'matreshka';
import config from "./config";

export default class Login extends MK.Object {
    constructor() {
        super();
        VK.init({apiId: config.vkAppId});
        
        this
            .jset({
                isHide: false,
                buttonText: 'Вход через ВК'
            })
            .bindNode({
                sandbox: '#login',
                button: ':sandbox button'
            })
            .bindNode('isHide', ':sandbox', MK.binders.display(false))
            .bindNode('buttonText', ':sandbox button', MK.binders.text())
            .on({
                'click::button': () => {
                    this.buttonText = 'Авторизация...';
                    VK.Auth.login(res => this.trigger('auth', res.session))
                }
            });
        
        VK.Auth.getLoginStatus(res => {
           if (res.status === 'connected') 
               this.trigger('auth', res.session);
        });
    }
    hide() {
        this.isHide = true;
    }
}
