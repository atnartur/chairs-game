import $ from 'jquery';
import MK from 'matreshka';
import Model from "./Model";

export default class Chairs extends MK.Array {
    get Model() {
        return Model;
    }
    get itemRenderer() {
        return '<img src="images/chair.png" class="chair">';
    }
    constructor() {
        super();
        this
            .set({
                isHide: true
            })
            .bindNode({
                sandbox: '#chairs',
                container: ":sandbox"
            })
            .bindNode('isHide', ':sandbox', MK.binders.display(false));
    }
    show() {
        this.isHide = false;
    }
    setPositions(count) {
        let w = this.$nodes.sandbox[0].offsetWidth;
        let h = this.$nodes.sandbox[0].offsetHeight;
        let xStart = w / 2;
        let yStart = h / 2;

        let chairs = [];

        this.widthCenter = Math.round(w / 2);
        this.heightCenter = Math.round(h / 2);
        
        let r = 0;
        
        if (count !== 1)
            r =  this.heightCenter / 2;

        for (let i = count-1; i >= 0; i--) {
            let a = i * (2 * Math.PI / count);
            let y = Math.round((Math.cos(a) * r + yStart));
            let x = this.widthCenter + Math.round((Math.sin(a) * r + xStart));
            chairs.push({
                rotate: (360 / count) * i, 
                x, 
                y
            });
        }
        
        console.log('chairs', chairs);
        
        this.recreate(chairs);
    }
}