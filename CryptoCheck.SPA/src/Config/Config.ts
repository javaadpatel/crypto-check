const envSettings = window as any;
export class Config {
  static cryptocheck_api_url = envSettings.CRYPTOCHECK_API_URL;
  static cryptocheck_api_code = envSettings.CRYPTOCHECK_API_CODE;
}
