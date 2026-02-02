export class PaymentTransaction {
    id: number = 0;
    orderId: string = "";
    providerReference?: string;
    amount: number = 0;
    status: number = 0;
    updatedOn?: Date;
}

export class PaymentTransactionCreate {
    amount?: number 
}

export class APIResponse {
    isError: boolean = false
    result: any
    statusCode: number = 200
    message: string = ""
}

export class UpdateStatus{
    providerReference: string = "";
    status: number = 0;
}