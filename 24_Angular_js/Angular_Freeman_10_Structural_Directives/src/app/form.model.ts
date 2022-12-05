import { FormControl, FormGroup, Validators } from "@angular/forms";
export class ProductFormControl extends FormControl {
    label: string;
    modelProperty: string;
    constructor(label: string, property: string, value: any, validator: any) {
        super(value, validator);
        this.label = label;
        this.modelProperty = property;
    }
    getValidationMessages() {
        let messages: string[] = [];
        if (this.errors) {
            for (let errorName in this.errors) {
                switch (errorName) {
                    case "required":
                        messages.push(`You must enter a ${this.label}`);
                        break;
                    case "minlength":
                        messages.push(`A ${this.label} must be at least
${this.errors['minlength'].requiredLength}
characters`);
                        break;
                    case "maxlength":
                        messages.push(`A ${this.label} must be no more than
${this.errors['maxlength'].requiredLength}
characters`);
                        break;
                    case "pattern":
                        messages.push(`The ${this.label} contains
illegal characters`);
                        break;
                }
            }
        }
        return messages;
    }
}
export class ProductFormGroup extends FormGroup {
    constructor() {
        super({
            name: new ProductFormControl("Name", "name", "", Validators.required),
            category: new ProductFormControl("Category", "category", "",
                Validators.compose([Validators.required,
                Validators.pattern("^[A-Za-z ]+$"),
                Validators.minLength(3),
                Validators.maxLength(10)])),
            price: new ProductFormControl("Price", "price", "",
                Validators.compose([Validators.required,
                Validators.pattern("^[0-9\.]+$")]))
        });
    }
    get productControls(): ProductFormControl[] {
        return Object.keys(this.controls)
            .map(k => this.controls[k] as ProductFormControl);
    }
    getValidationMessages(name: string): string[] {
        return (this.controls['name'] as ProductFormControl).getValidationMessages();
    }
    getFormValidationMessages(): string[] {
        let messages: string[] = [];
        Object.values(this.controls).forEach(c =>
            messages.push(...(c as ProductFormControl).getValidationMessages()));
        return messages;
    }
}