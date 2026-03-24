export interface Hours {
    today: Hour[];
    tomorrow: Hour[];
}

export interface Hour {
    hour: string;
    temperature: number;
}