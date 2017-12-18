import MK from 'matreshka';
import config from "./config";

export default class Login extends MK.Object {
    constructor() {
        super();
        VK.init({apiId: config.vkAppId});
        
        this
            .jset({
                isHide: false
            })
            .bindNode({
                sandbox: '#login',
                button: ':sandbox button'
            })
            .bindNode('isHide', ':sandbox', MK.binders.display(false))
            .on({
                'click::button': (evt) => VK.Auth.login(res => this.trigger('auth', res.session))
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
