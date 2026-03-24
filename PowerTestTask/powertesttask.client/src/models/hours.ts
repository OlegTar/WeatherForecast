export interface Hours {
    today: Hour[];
    tomorrow: Hour[];
}

interface Hour {
    hour: string;
    temperature: number;
}